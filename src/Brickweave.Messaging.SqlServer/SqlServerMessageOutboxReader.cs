using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Domain;
using Brickweave.Messaging.Models;
using Brickweave.Messaging.SqlServer.Entities;
using Brickweave.Messaging.SqlServer.Extensions;
using Brickweave.Serialization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer
{
    public class SqlServerMessageOutboxReader<TDbContext, TMessageData> : IMessageOutboxReader
        where TDbContext : DbContext
        where TMessageData : MessageData, new()
    {
        private readonly TDbContext _dbContext;
        private readonly DbSet<TMessageData> _dbSet;
        private readonly IDocumentSerializer _serializer;

        public SqlServerMessageOutboxReader(TDbContext dbContext, Func<TDbContext, DbSet<TMessageData>> getDbSet, 
            IDocumentSerializer serializer)
        {
            _dbContext = dbContext;
            _dbSet = getDbSet.Invoke(dbContext);
            _serializer = serializer;
        }

        public async Task<IEnumerable<DomainMessageInfo>> GetNextBatch(int batchSize, int retryAfterSeconds, int maxRetries)
        {
            var maxCountParam = new SqlParameter(
                "@maxCount", batchSize);
            var minRetryDateTimeParam = new SqlParameter(
                "@retryAfterSeconds", retryAfterSeconds);
            var maxRetriesParam = new SqlParameter(
                "@maxRetries", maxRetries);

            var sql = CreateDequeueQuery(maxCountParam);

            var data = await _dbSet
                .FromSqlRaw(sql, maxCountParam, minRetryDateTimeParam, maxRetriesParam)
                .AsNoTracking()
                .ToListAsync();

            return data
                .Select(m => new DomainMessageInfo(
                    new DomainMessageId(m.Id),
                    _serializer.DeserializeObject<IDomainEvent>(m.TypeName, m.Json),
                    m.Created,
                    m.CommitSequence,
                    m.SendAttemptCount,
                    m.LastSendAttempt))
                .ToList();
        }

        public async Task Delete(DomainMessageId domainMessageId)
        {
            var data = await _dbSet
                .Where(m => m.Id == domainMessageId.Value)
                .SingleOrDefaultAsync();

            _dbSet.Remove(data);

            await _dbContext.SaveChangesAsync();
        }

        public async Task ReportFailure(DomainMessageInfo domainMessage)
        {
            var data = await _dbSet
                .Where(m => m.Id == domainMessage.Id.Value)
                .SingleOrDefaultAsync();

            data.IsSending = false;
            data.SendAttemptCount = domainMessage.SendAttemptCount + 1;
            data.LastSendAttempt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        private static string CreateDequeueQuery(SqlParameter maxCountParam)
        {
            return string.Format(
                @"SET NOCOUNT ON;
	            
                DECLARE @results TABLE 
	            (
		            [Id] UNIQUEIDENTIFIER
                    , [TypeName] VARCHAR(200)
		            , [Json] VARCHAR(MAX)
		            , [Created] DATETIME
		            , [CommitSequence] INT
                    , [IsSending] BIT
                    , [SendAttemptCount] INT
                    , [LastSendAttempt] DATETIME
	            )

	            INSERT INTO @results (
                      [Id]
                    , [TypeName]
                    , [Json]
                    , [Created]
                    , [CommitSequence]
                    , [IsSending]
                    , [SendAttemptCount]
                    , [LastSendAttempt]
                )
	            SELECT TOP ({0}) 
                      [Id]
                    , [TypeName]
                    , [Json]
                    , [Created]
                    , [CommitSequence]
                    , [IsSending]
                    , [SendAttemptCount]
                    , [LastSendAttempt]
	            FROM 
                    [MessageOutbox] WITH (ROWLOCK, UPDLOCK, READPAST)
	            WHERE 
                    [isSending] = 0
                    AND (
                        [LastSendAttempt] IS NULL 
                        OR GETUTCDATE() >= DATEADD(second, @retryAfterSeconds, [LastSendAttempt])
                    )
                    AND [SendAttemptCount] < @maxRetries
                ORDER BY 
                      [Created]
                    , [CommitSequence]

	            UPDATE 
                    [MessageOutbox]
	            SET 
                    [IsSending] = 1
	            WHERE 
                    [Id] IN (SELECT [Id] FROM @results)
	
	            SELECT * from @results",
                maxCountParam.Value).TrimExtraWhitespace();
        }
    }
}
