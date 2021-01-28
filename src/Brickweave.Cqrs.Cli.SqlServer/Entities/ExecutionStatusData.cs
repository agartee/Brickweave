using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickweave.Cqrs.Cli.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class ExecutionStatusData
    {
        public const string TABLE_NAME = "ExecutionStatus";

        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
    }
}
