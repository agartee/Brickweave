using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickweave.Cqrs.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class CommandStatusData
    {
        public const string TABLE_NAME = "CommandQueue"; // intentional

        public Guid Id { get; set; }
        [Column("ResultTypeName"), MaxLength(200)]
        public string ResultTypeName { get; set; }
        [Column("ResultJson")]
        public string ResultJson { get; set; }
        [Column("Started")] 
        public DateTime? Started { get; set; }
        [Column("Completed")] 
        public DateTime? Completed { get; set; }

        [ForeignKey("Id")]
        public CommandQueueData CommandQueue { get; set; }
    }
}
