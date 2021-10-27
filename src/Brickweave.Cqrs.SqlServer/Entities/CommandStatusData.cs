using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickweave.Cqrs.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class CommandStatusData
    {
        public const string TABLE_NAME = "CommandStatus";

        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        [MaxLength(200)]
        public string ContentType { get; set; }
        public string Content { get; set; }
    }
}
