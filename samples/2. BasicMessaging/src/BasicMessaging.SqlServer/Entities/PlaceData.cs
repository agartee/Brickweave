using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicMessaging.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class PlaceData
    {
        public const string TABLE_NAME = "Place";

        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
    }
}
