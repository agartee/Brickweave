using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Serialization;
using Microsoft.EntityFrameworkCore;
using Brickweave.Cqrs.Models;
using Brickweave.Cqrs.SqlServer.Entities;
using Brickweave.Cqrs.Services;
using Brickweave.Cqrs.SqlServer.Extensions;

namespace Brickweave.Cqrs.SqlServer.Services
{
    public class SqlServerCommandQueue<TDbContext> : ICommandQueue where TDbContext : DbContext
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<CommandQueueData> _commandQueueDbSet;
        private readonly IDocumentSerializer _serializer;
        private readonly string _schemaName;

        public SqlServerCommandQueue(TDbContext dbContext, Func<TDbContext, DbSet<CommandQueueData>> getCommandQueueDbSet, IDocumentSerializer serializer)
        {
            _dbContext = dbContext;
            _commandQueueDbSet = getCommandQueueDbSet.Invoke(dbContext);
            _serializer = serializer;
        }

        public async Task EnqueueCommandAsync(Guid commandId, ICommand executable, ClaimsPrincipalInfo user = null)
        {
            _commandQueueDbSet.Add(new CommandQueueData
            {
                Id = commandId,
                TypeName = executable.GetType().AssemblyQualifiedName,
                CommandJson = _serializer.SerializeObject(executable),
                ClaimsPrincipalJson = user != null ? _serializer.SerializeObject(user) : null,
                Created = DateTime.UtcNow,
                IsProcessing = false
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task<CommandInfo> GetNext()
        {
            var sql = CreateDequeueQuery();

            var data = (await _commandQueueDbSet
                .FromSqlRaw(sql)
                .ToListAsync())
                .FirstOrDefault();

            if (data == null)
                return null;

            var command = (ICommand)_serializer.DeserializeObject(data.TypeName, data.CommandJson);
            var principal = data.ClaimsPrincipalJson != null
                ? _serializer.DeserializeObject<ClaimsPrincipalInfo>(data.ClaimsPrincipalJson).ToClaimsPrincipal()
                : null;

            return new CommandInfo(data.Id, command, principal);
        }

        public async Task Delete(Guid commandId)
        {
            var data = await _commandQueueDbSet
                .FirstOrDefaultAsync(c => c.Id == commandId);

            if (data == null)
                return;

            _commandQueueDbSet.Remove(data);
            await _dbContext.SaveChangesAsync();
        }

        private string CreateDequeueQuery()
        {
            var schema = _dbContext.Model.FindEntityType(typeof(CommandQueueData)).GetSchema();
             
            var sql = string.Format(
                $@"SET NOCOUNT ON;
	            
                DECLARE @results TABLE 
	            (
		            [Id] UNIQUEIDENTIFIER
                    , [TypeName] VARCHAR(200)
		            , [CommandJson] VARCHAR(MAX)
                    , [ClaimsPrincipalJson] VARCHAR(MAX)
		            , [Created] DATETIME
                    , [IsProcessing] BIT
	            )

	            INSERT INTO @results ([Id], [TypeName], [CommandJson], [ClaimsPrincipalJson], [Created], [IsProcessing])
	            SELECT TOP (1) [Id], [TypeName], [CommandJson], [ClaimsPrincipalJson], [Created], [IsProcessing]
	            FROM [{ schema }].[{ CommandQueueData.TABLE_NAME }] WITH (ROWLOCK, READPAST)
	            WHERE [isProcessing] = 0
                ORDER BY [Created]

	            UPDATE [{ schema }].[{ CommandQueueData.TABLE_NAME }]
	            SET [IsProcessing] = 1
	            WHERE [Id] IN (SELECT [Id] FROM @results)
	
	            SELECT * from @results");

            return sql.TrimExtraWhitespace();
        }
    }
}
