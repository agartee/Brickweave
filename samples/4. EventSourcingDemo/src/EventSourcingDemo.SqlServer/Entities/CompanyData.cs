using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSourcingDemo.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class CompanyData
    {
        public const string TABLE_NAME = "Company";

        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }

        public List<BusinessAccountData> Accounts { get; set; } = new List<BusinessAccountData>();
    }
}
