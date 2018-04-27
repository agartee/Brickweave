using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickweave.Messaging.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class MessageFailureData
    {
        public const string TABLE_NAME = "MessageFailure";

        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public DateTime Enqueued { get; set; }
    }
}