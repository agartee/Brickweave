using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventSourcing.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class IdeaData
    {
        public const string TABLE_NAME = "Idea";

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
