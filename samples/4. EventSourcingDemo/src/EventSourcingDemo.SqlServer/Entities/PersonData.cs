using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSourcingDemo.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class PersonData
    {
        public const string TABLE_NAME = "Person";

        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }

        public List<PersonalAccountData> Accounts { get; set; } = new List<PersonalAccountData>();
    }
}
