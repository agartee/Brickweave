using System.Linq;
using System.Security.Claims;
using Brickweave.Cqrs.Models;

namespace Brickweave.Cqrs.SqlServer.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static ClaimsPrincipal ToClaimsPrincipal(this ClaimsPrincipalInfo principal)
        {
            var claims = principal.Claims
                .Select(c => new Claim(c.Type, c.Value))
                .ToList();

            var identity = new ClaimsIdentity(
                claims,
                principal.AuthenticationType,
                ClaimTypes.Name,
                ClaimTypes.Role);

            return new ClaimsPrincipal(identity);
        }
    }
}
