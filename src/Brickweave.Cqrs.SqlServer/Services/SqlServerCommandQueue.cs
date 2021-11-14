using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Serialization;
using Microsoft.EntityFrameworkCore;
using Brickweave.Cqrs.Models;
using Brickweave.Cqrs.SqlServer.Entities;
using Brickweave.Cqrs.Services;
using Brickweave.Cqrs.SqlServer.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Brickweave.Cqrs.SqlServer.Services
{
    public class SqlServerCommandQueue<TDbContext> : ICommandQueue where TDbContext : DbContext
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<CommandQueueData> _commandQueueDbSet;
        private readonly IDocumentSerializer _serializer;
        private readonly ILogger<SqlServerCommandQueue<TDbContext>> _logger;
        public SqlServerCommandQueue(TDbContext dbContext, Func<TDbContext, DbSet<CommandQueueData>> getCommandQueueDbSet, IDocumentSerializer serializer, 
            ILogger<SqlServerCommandQueue<TDbContext>> logger)
        {
            _dbContext = dbContext;
            _commandQueueDbSet = getCommandQueueDbSet.Invoke(dbContext);
            _serializer = serializer;
            _logger = logger;
        }

        public async Task EnqueueCommandAsync(Guid commandId, ICommand executable, ClaimsPrincipalInfo user = null)
        {
            _commandQueueDbSet.Add(new CommandQueueData
            {
                Id = commandId,
                CommandTypeName = executable.GetType().AssemblyQualifiedName,
                CommandJson = _serializer.SerializeObject(executable),
                ClaimsPrincipalJson = user != null ? _serializer.SerializeObject(user) : null,
                Created = DateTime.UtcNow
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task<CommandInfo> GetNextAsync()
        {
            var sql = CreateDequeueQuery();
            var startedParameter = new SqlParameter("@started", DateTime.UtcNow);

            var data = (await _commandQueueDbSet
                .FromSqlRaw(sql, new[] { startedParameter })
                .AsNoTracking()
                .ToListAsync())
                .FirstOrDefault();

            if (data == null)
                return null;

            var command = (ICommand) _serializer.DeserializeObject(data.CommandTypeName, data.CommandJson);
            var principal = data.ClaimsPrincipalJson != null
                ? _serializer.DeserializeObject<ClaimsPrincipalInfo>(data.ClaimsPrincipalJson).ToClaimsPrincipal()
                : null;

            return new CommandInfo(data.Id, command, principal);
        }

        public async Task ReportCompletedAsync(Guid commandId, object result = null)
        {
            var data = await _commandQueueDbSet.SingleOrDefaultAsync(s => s.Id == commandId);

            if (data == null)
                throw new InvalidOperationException($"Command with ID \"{commandId}\" was not found.");

            data.Completed = DateTime.UtcNow;
            data.ResultTypeName = result.GetType().AssemblyQualifiedName;
            data.ResultJson = result != null 
                ? _serializer.SerializeObject(result) 
                : null;

            await _dbContext.SaveChangesAsync();

            _logger.LogDebug($"Command with ID { commandId } reported complete with a result of: { Environment.NewLine + data.ResultJson }");
        }

        public async Task ReportExceptionAsync(Guid commandId, Exception exception)
        {
            var data = await _commandQueueDbSet.SingleOrDefaultAsync(s => s.Id == commandId);

            if (data == null)
                throw new InvalidOperationException($"Command with ID \"{commandId}\" was not found.");

            data.Completed = DateTime.UtcNow;
            data.ResultTypeName = typeof(ExceptionInfo).AssemblyQualifiedName;
            data.ResultJson = _serializer.SerializeObject(new ExceptionInfo(exception.GetType().Name, exception.Message));

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commandId)
        {
            var data = await _commandQueueDbSet
                .FirstOrDefaultAsync(c => c.Id == commandId);

            if (data == null)
                return;

            _commandQueueDbSet.Remove(data);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOlderThanAsync(TimeSpan deleteAfter)
        {
            var sql = CreateCleanupQuery();
            var deleteAfterParameter = new SqlParameter("@deleteAfterMilliseconds", deleteAfter.TotalMilliseconds);

            await _dbContext.Database.ExecuteSqlRawAsync(sql, deleteAfterParameter);
        }

        private string CreateDequeueQuery()
        {
            var schema = _dbContext.Model
                .FindEntityType(typeof(CommandQueueData))
                .GetSchema();
             
            var sql = string.Format(
                $@"SET NOCOUNT ON;
	            
                DECLARE @results TABLE 
	            (
		            [Id] UNIQUEIDENTIFIER
                    , [CommandTypeName] VARCHAR(200)
		            , [CommandJson] VARCHAR(MAX)
                    , [ClaimsPrincipalJson] VARCHAR(MAX)
		            , [Created] DATETIME
	            )

	            INSERT INTO @results ([Id], [CommandTypeName], [CommandJson], [ClaimsPrincipalJson], [Created])
	            SELECT TOP (1) 
                    [Id]
                    , [CommandTypeName]
                    , [CommandJson]
                    , [ClaimsPrincipalJson]
                    , [Created]

	            FROM [{ schema }].[{ CommandQueueData.TABLE_NAME }] WITH (ROWLOCK, READPAST)
	            WHERE [Started] IS NULL
                ORDER BY [Created]

	            UPDATE [{ schema }].[{ CommandQueueData.TABLE_NAME }]
	            SET [Started] = @started
	            WHERE 
                    [Id] IN (SELECT [Id] FROM @results)
	
	            SELECT 
                    [Id]
                    , [CommandTypeName]
                    , [CommandJson]
                    , [ClaimsPrincipalJson]
                    , NULL [ResultTypeName]
                    , NULL [ResultJson]
                    , [Created]
                    , NULL [Started]
                    , NULL [Completed]
                FROM 
                    @results");

            return sql.TrimExtraWhitespace();
        }

        private string CreateCleanupQuery()
        {
            var schema = _dbContext.Model
                .FindEntityType(typeof(CommandQueueData))
                .GetSchema();

            var sql = string.Format(
                $@"SET NOCOUNT ON;
	            
                DELETE FROM [{ schema }].[{ CommandQueueData.TABLE_NAME }]
                WHERE 
                    DATEDIFF(millisecond, [Started], GETUTCDATE()) >= @deleteAfterMilliseconds
	            ");

            return sql.TrimExtraWhitespace();
        }
    }
}
