using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Domain;
using Brickweave.Messaging.Models;
using Brickweave.Messaging.SqlServer.Entities;
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

        public async Task<IEnumerable<DomainMessageInfo>> GetNextBatch(int batchSize)
        {
            var maxCountParam = new SqlParameter(
                "@maxCount", batchSize);

            var sql = CreateDequeueQuery(maxCountParam);

            var data = await _dbSet
                .FromSqlRaw(sql, maxCountParam)
                .ToListAsync();

            return data
                .Select(m => new DomainMessageInfo(
                    new DomainMessageId(m.Id),
                    _serializer.DeserializeObject<IDomainEvent>(m.Json),
                    m.Created,
                    m.CommitSequence
                    ))
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

        private static string CreateDequeueQuery(SqlParameter maxCountParam)
        {
            return string.Format(
                @"SET NOCOUNT ON;
	            
                DECLARE @results TABLE 
	            (
		            [Id] UNIQUEIDENTIFIER
		            , [Json] VARCHAR(MAX)
		            , [Created] DATETIME
		            , [CommitSequence] INT
                    , [IsSending] BIT
	            )

	            INSERT INTO @results ([Id], [Json], [Created], [CommitSequence], [IsSending])
	            SELECT TOP ({0}) [Id], [Json], [Created], [CommitSequence], [IsSending]
	            FROM [MessageOutbox] WITH (ROWLOCK, READPAST)
	            WHERE [isSending] = 0
                ORDER BY [Created], [CommitSequence]

	            UPDATE [MessageOutbox]
	            SET [IsSending] = 1
	            WHERE [Id] IN (SELECT [Id] FROM @results)
	
	            SELECT * from @results",
                maxCountParam.Value);
        }
    }
}
