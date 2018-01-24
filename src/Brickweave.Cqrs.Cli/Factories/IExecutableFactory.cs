using System;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
{
    public interface IExecutableFactory
    {
        bool Exists(string name);
        IExecutable Create(ExecutableInfo executableInfo);
        IExecutable Create(Type type, params ExecutableParameterInfo[] parameterValues);
    }
}
