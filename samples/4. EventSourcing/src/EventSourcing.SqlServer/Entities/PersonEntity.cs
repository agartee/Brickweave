using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSourcing.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class PersonEntity
    {
        public const string TABLE_NAME = "Place";

        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
