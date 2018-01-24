using System;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
{
    public interface IParameterValueFactory
    {
        bool Qualifies(Type targetType);
        object Create(Type targetType, ExecutableParameterInfo parameter);
    }

    public interface ISingleParameterValueFactory : IParameterValueFactory
    {
        
    }
}
