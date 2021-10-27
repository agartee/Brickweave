using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvancedCqrs.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class ThingData
    {
        public const string TABLE_NAME = "Brand";

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
