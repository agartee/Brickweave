using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickweave.EventStore.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class EventData
    {
        public const string TABLE_NAME = "Event";

        public Guid Id { get; set; }
        [MaxLength(200)]
        public string TypeName { get; set; }
        public Guid StreamId { get; set; }
        public string Json { get; set; }
        public DateTime Created { get; set; }
        public int CommitSequence { get; set; }
    }
}
