using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Services;
using Brickweave.Cqrs.Cli.SqlServer.Entities;
using Brickweave.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Cqrs.Cli.SqlServer.Services
{
    public class SqlServerCommandStatusRepository<TDbContext> : ICommandStatusRepository where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<CommandQueueData> _commandQueueDbSet;
        private readonly DbSet<CommandStatusData> _commandStatusDbSet;
        private readonly IDocumentSerializer _documentSerializer;

        public SqlServerCommandStatusRepository(TDbContext dbContext, Func<TDbContext, DbSet<CommandQueueData>> getCommandQueueDbSet, 
            Func<TDbContext, DbSet<CommandStatusData>> getCommandStatusDbSet, IDocumentSerializer documentSerializer)
        {
            _dbContext = dbContext;
            _commandQueueDbSet = getCommandQueueDbSet.Invoke(dbContext);
            _commandStatusDbSet = getCommandStatusDbSet.Invoke(dbContext);
            _documentSerializer = documentSerializer;
        }

        public async Task ReportStartedAsync(Guid executionId)
        {
            _commandStatusDbSet.Add(new CommandStatusData
            {
                Id = executionId,
                Start = DateTime.UtcNow
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task ReportCompletedAsync(Guid executionId, object result = null)
        {
            var data = await _commandStatusDbSet.SingleOrDefaultAsync(s => s.Id == executionId);

            if (data == null)
                throw new InvalidOperationException($"Unable to report execution completed. Execution with ID \"{executionId}\" was not found.");

            data.End = DateTime.UtcNow;
            data.ContentType = result.GetType().AssemblyQualifiedName;
            data.Content = result != null ? _documentSerializer.SerializeObject(result) : null;

            await _dbContext.SaveChangesAsync();
        }

        public async Task ReportErrorAsync(Guid executionId, Exception exception)
        {
            var data = await _commandStatusDbSet.SingleOrDefaultAsync(s => s.Id == executionId);

            if (data == null)
                throw new InvalidOperationException($"Unable to report execution completed. Execution with ID \"{executionId}\" was not found.");

            data.End = DateTime.UtcNow;
            data.ContentType = nameof(Exception);
            data.Content = exception.Message;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IExecutionStatus> ReadStatusAsync(Guid executionId)
        {
            var data = await _commandStatusDbSet.SingleOrDefaultAsync(s => s.Id == executionId);

            if (data == null)
                return new NotFoundStatus(executionId);

            if (data.ContentType == null)
                return new RunningStatus(executionId, data.Start);
            if (data.ContentType == nameof(Exception))
                return new ErrorStatus(executionId, data.Start, data.End.Value, data.Content);
            if (data.ContentType != null)
            {
                var result = _documentSerializer.DeserializeObject(data.ContentType, data.Content);
                var status = new CompletedStatus(executionId, data.Start, data.End.Value, result);

                // cleanup
                _commandStatusDbSet.Remove(data);
                var queuedCommandData = await _commandQueueDbSet
                    .FirstOrDefaultAsync(cancellationToken => cancellationToken.Id == data.Id);

                if (queuedCommandData != null)
                    _commandQueueDbSet.Remove(queuedCommandData);

                await _dbContext.SaveChangesAsync();
                return status;
            }

            throw new InvalidOperationException($"Unknown status of Execution with ID \"{executionId}\"");
        }
    }
}
