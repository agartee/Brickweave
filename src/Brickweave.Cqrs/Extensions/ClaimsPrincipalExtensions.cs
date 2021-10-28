using System.Linq;
using System.Security.Claims;
using Brickweave.Cqrs.Models;

namespace Brickweave.Cqrs.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static ClaimsPrincipalInfo ToInfo(this ClaimsPrincipal principal)
        {
            return new ClaimsPrincipalInfo(
                principal.Identity.AuthenticationType,
                principal.Claims
                .Select(c => c.ToInfo())
                .ToList());
        }
    }
}
