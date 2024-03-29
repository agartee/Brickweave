﻿using System.Security.Claims;

namespace Brickweave.Cqrs.Models
{
    public class ClaimInfo
    {
        public ClaimInfo(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; }
        public string Value { get; }
    }
}
