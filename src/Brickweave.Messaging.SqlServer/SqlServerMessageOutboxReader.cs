using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Domain;
using Brickweave.Messaging.SqlServer.Entities;
using Brickweave.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer
{
    public class SqlServerMessageOutboxReader<TMessageData> : IMessageOutboxReader
        where TMessageData : MessageData, new()
    {
        private readonly DbSet<TMessageData> _dbSet;
        private readonly IDocumentSerializer _serializer;

        public SqlServerMessageOutboxReader(DbSet<TMessageData> dbSet, IDocumentSerializer serializer)
        {
            _dbSet = dbSet;
            _serializer = serializer;
        }

        public async Task<IEnumerable<IDomainEvent>> GetNextBatch(int maxCount)
        {
            var maxCountParam = new SqlParameter(
                "@maxCount", maxCount);

            var sql = CreateDequeueCommand(maxCountParam);

            var data = _dbSet.FromSql(sql, maxCountParam);

            return data
                .Select(m => _serializer.DeserializeObject<IDomainEvent>(m.Json))
                .ToList();
        }

        private static string CreateDequeueCommand(SqlParameter maxCountParam)
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
	            where [isSending] = 0

	            UPDATE [MessageOutbox]
	            SET [IsSending] = 1
	            WHERE [Id] IN (SELECT [Id] FROM @results)
	
	            SELECT * from @results",
                maxCountParam.Value);
        }
    }
}
