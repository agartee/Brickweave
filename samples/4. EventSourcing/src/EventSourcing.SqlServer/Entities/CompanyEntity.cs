using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSourcing.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class CompanyEntity
    {
        public const string TABLE_NAME = "Company";

        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
