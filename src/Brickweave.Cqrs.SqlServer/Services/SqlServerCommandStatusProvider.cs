using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Models;
using Brickweave.Cqrs.Services;
using Brickweave.Cqrs.SqlServer.Entities;
using Brickweave.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Cqrs.SqlServer.Services
{
    public class SqlServerCommandStatusProvider<TDbContext> : ICommandStatusProvider where TDbContext : DbContext
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<CommandStatusData> _commandQueueDbSet;
        private readonly IDocumentSerializer _serializer;

        public SqlServerCommandStatusProvider(TDbContext dbContext, Func<TDbContext, DbSet<CommandStatusData>> getCommandStatusDbSet, IDocumentSerializer serializer)
        {
            _dbContext = dbContext;
            _commandQueueDbSet = getCommandStatusDbSet.Invoke(dbContext);
            _serializer = serializer;
        }

        public async Task<IExecutionStatus> GetStatusAsync(Guid commandId)
        {
            var data = await _commandQueueDbSet
                .SingleOrDefaultAsync(s => s.Id == commandId);

            if(data == null)
                return new CommandNotFoundExecutionStatus(commandId);

            if (data.Started == null)
                return new CommandNotStartedExecutionStatus(commandId);

            if (data.Started != null && data.Completed == null)
                return new CommandRunningExecutionStatus(commandId, data.Started.Value);

            if (data.Completed != null)
            {
                var result = data.ResultTypeName != null
                    ? _serializer.DeserializeObject(data.ResultTypeName, data.ResultJson)
                    : null;
                var status = result is ExceptionInfo
                    ? (ExecutionStatus) new CommandErrorExecutionStatus(commandId, data.Started.Value, data.Completed.Value, (ExceptionInfo) result)
                    : new CommandCompletedExecutionStatus(commandId, data.Started.Value, data.Completed.Value, result);

                _commandQueueDbSet.Remove(data);
                await _dbContext.SaveChangesAsync();

                return status;
            }

            throw new InvalidOperationException($"An unhandled result occurred in { nameof(SqlServerCommandStatusProvider<TDbContext>) }.{ nameof(GetStatusAsync) }");
        }
    }
}
