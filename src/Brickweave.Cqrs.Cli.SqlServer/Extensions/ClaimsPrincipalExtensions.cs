using System.Linq;
using System.Security.Claims;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.SqlServer.Extensions
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

        public static ClaimsPrincipalInfo ToClaimsPrincipalInfo(this ClaimsPrincipal principal)
        {
            return new ClaimsPrincipalInfo(
                principal.Identity.AuthenticationType,
                principal.Claims
                .Select(c => c.ToInfo())
                .ToList());
        }
    }
}
