using System;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories.ParameterValues
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
