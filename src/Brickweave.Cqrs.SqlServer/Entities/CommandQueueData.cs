using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickweave.Cqrs.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class CommandQueueData
    {
        public const string TABLE_NAME = "CommandQueue";

        public Guid Id { get; set; }
        [MaxLength(200)]
        public string CommandTypeName { get; set; }
        public string CommandJson { get; set; }
        public string ClaimsPrincipalJson { get; set; }
        [MaxLength(200)]
        public string ResultTypeName { get; set; }
        public string ResultJson { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Completed { get; set; }
    }
}
