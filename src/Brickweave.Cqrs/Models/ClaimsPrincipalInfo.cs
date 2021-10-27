using System.Collections.Generic;
using System.Linq;

namespace Brickweave.Cqrs.Models
{
    public class ClaimsPrincipalInfo
    {
        public ClaimsPrincipalInfo(string authenticationType, IEnumerable<ClaimInfo> claims)
        {
            AuthenticationType = authenticationType;
            Claims = claims;
        }

        public string AuthenticationType { get; }
        public IEnumerable<ClaimInfo> Claims { get; } = Enumerable.Empty<ClaimInfo>();
    }
}
