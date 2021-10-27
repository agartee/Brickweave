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
        public string TypeName { get; set; }
        public string CommandJson { get; set; }
        public string ClaimsPrincipalJson { get; set; }
        public DateTime Created { get; set; }
        public bool IsProcessing { get; set; }
    }
}
