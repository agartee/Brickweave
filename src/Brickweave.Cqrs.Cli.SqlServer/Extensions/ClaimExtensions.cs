using System.Security.Claims;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.SqlServer.Extensions
{
    public static class ClaimExtensions
    {
        public static ClaimInfo ToInfo(this Claim claim)
        {
            return new ClaimInfo(claim.Type, claim.Value);
        }
    }
}
