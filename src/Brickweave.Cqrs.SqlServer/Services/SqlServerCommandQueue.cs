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

namespace Brickweave.Cqrs.SqlServer.Services
{
    /// <summary>
    /// The Command Queue service is intended to be long-running and therefore requires a different way of managing DbContext interactions.
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public class SqlServerCommandQueue<TDbContext> : ICommandQueue where TDbContext : DbContext
    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;
        private readonly Func<TDbContext, DbSet<CommandQueueData>> _commandQueueDbSet;
        private readonly Func<TDbContext, DbSet<CommandStatusData>> _commandStatusDbSet;
        private readonly IDocumentSerializer _serializer;

        public SqlServerCommandQueue(IDbContextFactory<TDbContext> dbContextFactory, Func<TDbContext, DbSet<CommandQueueData>> getCommandQueueDbSet,
            Func<TDbContext, DbSet<CommandStatusData>> getCommandStatusDbSet, IDocumentSerializer serializer)
        {
            _dbContextFactory = dbContextFactory;
            _commandQueueDbSet = getCommandQueueDbSet;
            _commandStatusDbSet = getCommandStatusDbSet;
            _serializer = serializer;
        }

        public async Task EnqueueCommandAsync(Guid commandId, ICommand executable, ClaimsPrincipalInfo user = null)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var dbSet = _commandQueueDbSet.Invoke(dbContext);

            dbSet.Add(new CommandQueueData
            {
                Id = commandId,
                CommandTypeName = executable.GetType().AssemblyQualifiedName,
                CommandJson = _serializer.SerializeObject(executable),
                ClaimsPrincipalJson = user != null ? _serializer.SerializeObject(user) : null,
                Created = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
        }

        public async Task<CommandInfo> GetNextAsync()
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var dbSet = _commandQueueDbSet.Invoke(dbContext);
            var schema = dbContext.Model
                .FindEntityType(typeof(CommandQueueData))
                .GetSchema();

            var sql = CreateDequeueQuery(schema);
            var startedParameter = new SqlParameter("@started", DateTime.UtcNow);

            var data = (await dbSet
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
            using var dbContext = _dbContextFactory.CreateDbContext();
            var dbSet = _commandStatusDbSet.Invoke(dbContext);

            var data = await dbSet.SingleOrDefaultAsync(s => s.Id == commandId);

            if (data == null)
                throw new InvalidOperationException($"Command with ID \"{commandId}\" was not found.");

            data.Completed = DateTime.UtcNow;
            data.ResultTypeName = result.GetType().AssemblyQualifiedName;
            data.ResultJson = result != null 
                ? _serializer.SerializeObject(result) 
                : null;

            await dbContext.SaveChangesAsync();
        }

        public async Task ReportExceptionAsync(Guid commandId, Exception exception)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var dbSet = _commandStatusDbSet.Invoke(dbContext);

            var data = await dbSet.SingleOrDefaultAsync(s => s.Id == commandId);

            if (data == null)
                throw new InvalidOperationException($"Command with ID \"{commandId}\" was not found.");

            data.Completed = DateTime.UtcNow;
            data.ResultTypeName = typeof(ExceptionInfo).AssemblyQualifiedName;
            data.ResultJson = _serializer.SerializeObject(new ExceptionInfo(exception.GetType().Name, exception.GetFullMessage()));

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commandId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var dbSet = _commandStatusDbSet.Invoke(dbContext);

            var data = await dbSet
                .FirstOrDefaultAsync(c => c.Id == commandId);

            if (data == null)
                return;

            dbSet.Remove(data);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteOlderThanAsync(TimeSpan deleteAfter)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();
            var schema = dbContext.Model
                .FindEntityType(typeof(CommandQueueData))
                .GetSchema();

            var sql = CreateCleanupQuery(schema);
            var deleteAfterParameter = new SqlParameter("@deleteAfterMilliseconds", deleteAfter.TotalMilliseconds);

            await dbContext.Database.ExecuteSqlRawAsync(sql, deleteAfterParameter);
        }

        private string CreateDequeueQuery(string schema)
        {
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

        private string CreateCleanupQuery(string schema)
        {
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
