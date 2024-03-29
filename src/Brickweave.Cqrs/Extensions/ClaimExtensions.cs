﻿using System.Security.Claims;
using Brickweave.Cqrs.Models;

namespace Brickweave.Cqrs.Extensions
{
    public static class ClaimExtensions
    {
        public static ClaimInfo ToInfo(this Claim claim)
        {
            return new ClaimInfo(claim.Type, claim.Value);
        }
    }
}
