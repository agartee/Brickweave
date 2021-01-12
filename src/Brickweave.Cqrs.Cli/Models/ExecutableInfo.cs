using System.Collections.Generic;
using LiteGuard;

namespace Brickweave.Cqrs.Cli.Models
{
    public class ExecutableInfo
    {
        public ExecutableInfo(string name, IEnumerable<ExecutableParameterInfo> parameters)
        {
            Guard.AgainstNullArgument(nameof(name), name);
            Guard.AgainstNullArgument(nameof(parameters), parameters);

            Name = name;
            Parameters = parameters;
        }

        public string Name { get; }
        public IEnumerable<ExecutableParameterInfo> Parameters { get; }
    }
}
