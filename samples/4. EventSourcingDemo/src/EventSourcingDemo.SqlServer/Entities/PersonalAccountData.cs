using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSourcingDemo.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class PersonalAccountData
    {
        public const string TABLE_NAME = "PersonalAccount";

        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public Guid AccountHolderId { get; set; }
        public decimal Balance { get; set; }

        public PersonData AccountHolder { get; set; }
    }
}
