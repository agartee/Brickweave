using System;
using System.Collections.Generic;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
{
    public interface IExecutableFactory
    {
        IExecutable Create(ExecutableInfo executableInfo);
        IExecutable Create(Type type, Dictionary<string, string> parameterValues);
    }
}
