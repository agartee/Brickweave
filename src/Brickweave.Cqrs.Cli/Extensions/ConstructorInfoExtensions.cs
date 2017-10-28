using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Brickweave.Cqrs.Cli.Extensions
{
    public static class ConstructorInfoExtensions
    {
        public static bool ContainsAllParameters(this ConstructorInfo ctor, IEnumerable<string> parameterNames)
        {
            var paramNames = parameterNames
                .Select(p => p.ToLowerInvariant())
                .ToArray();

            var ctorParamNames = ctor.GetParameters()
                .Select(p => p.Name.ToLowerInvariant())
                .ToArray();
            
            return paramNames.All(ctorParamNames.Contains);
        }
    }
}
