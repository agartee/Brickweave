using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Repositories;
using Brickweave.Cqrs.Cli.SqlServer.Entities;
using Brickweave.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Cqrs.Cli.SqlServer.Repositories
{
    public class ExecutionStatusRepository<TDbContext> : IExecutionStatusRepository
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<ExecutionStatusData> _dbSet;
        private readonly IDocumentSerializer _documentSerializer;

        public ExecutionStatusRepository(TDbContext dbContext, Func<TDbContext, DbSet<ExecutionStatusData>> getDbSet,
            IDocumentSerializer documentSerializer)
        {
            _dbContext = dbContext;
            _dbSet = getDbSet.Invoke(dbContext);
            _documentSerializer = documentSerializer;
        }

        public async Task ReportStartedAsync(Guid executionId)
        {
            _dbSet.Add(new ExecutionStatusData
            {
                Id = executionId,
                Start = DateTime.UtcNow
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task ReportCompletedAsync(Guid executionId, object result = null)
        {
            var data = await _dbSet.SingleOrDefaultAsync(s => s.Id == executionId);

            if (data == null)
                throw new InvalidOperationException($"Unable to report execution completed. Execution with ID \"{executionId}\" does was not found.");

            data.End = DateTime.UtcNow;
            data.ContentType = result.GetType().AssemblyQualifiedName;
            data.Content = result != null ? _documentSerializer.SerializeObject(result) : null;

            await _dbContext.SaveChangesAsync();
        }

        public async Task ReportErrorAsync(Guid executionId, Exception exception)
        {
            var data = await _dbSet.SingleOrDefaultAsync(s => s.Id == executionId);

            if (data == null)
                throw new InvalidOperationException($"Unable to report execution completed. Execution with ID \"{executionId}\" does was not found.");

            data.End = DateTime.UtcNow;
            data.ContentType = nameof(Exception);
            data.Content = exception.Message;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IExecutionStatus> ReadStatusAsync(Guid executionId)
        {
            var data = await _dbSet.SingleOrDefaultAsync(s => s.Id == executionId);

            if (data == null)
                return new NotFoundStatus(executionId);

            if (data.ContentType == null)
                return new RunningStatus(data.Id, data.Start);
            if (data.ContentType == nameof(Exception))
                return new ErrorStatus(data.Id, data.Start, data.End.Value, data.Content);
            if (data.ContentType != null)
            {
                var result = _documentSerializer.DeserializeObject(data.ContentType, data.Content);
                return new CompletedStatus(data.Id, data.Start, data.End.Value, result);
            }

            throw new InvalidOperationException($"Unknown status of Execution with ID \"{executionId}\"");
        }
    }
}
