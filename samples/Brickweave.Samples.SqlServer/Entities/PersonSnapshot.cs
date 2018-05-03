using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickweave.Samples.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class PersonSnapshot
    {
        public const string TABLE_NAME = "Person";

        public Guid Id { get; set; }
        public string Json { get; set; }
    }
}