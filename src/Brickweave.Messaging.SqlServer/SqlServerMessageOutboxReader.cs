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
    public class SqlServerMessageOutboxReader : IMessageOutboxReader
    {
        private readonly IDocumentSerializer _serializer;

        public SqlServerMessageOutboxReader(IDocumentSerializer serializer)
        {
            _serializer = serializer;
        }

        public async Task<IEnumerable<IDomainEvent>> GetNextBatch<TMessageData>(DbSet<TMessageData> dbSet, int batchSize)
             where TMessageData : MessageData, new()
        {
            var maxCountParam = new SqlParameter(
                "@maxCount", batchSize);

            var sql = CreateDequeueCommand(maxCountParam);

            var data = await dbSet
                .FromSql(sql, maxCountParam)
                .ToListAsync();

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
