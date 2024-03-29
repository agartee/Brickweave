﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickweave.Cqrs.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class CommandQueueData
    {
        public const string TABLE_NAME = "CommandQueue";

        public Guid Id { get; set; }
        [MaxLength(200)]
        public string CommandTypeName { get; set; }
        public string CommandJson { get; set; }
        public string ClaimsPrincipalJson { get; set; }
        [Column("ResultTypeName"), MaxLength(200)]
        public string ResultTypeName { get; set; }
        [Column("ResultJson")] 
        public string ResultJson { get; set; }
        public DateTime Created { get; set; }
        [Column("Started")] 
        public DateTime? Started { get; set; }
        [Column("Completed")] 
        public DateTime? Completed { get; set; }
    }
}
