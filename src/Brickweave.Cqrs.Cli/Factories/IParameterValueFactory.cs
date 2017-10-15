using System;

namespace Brickweave.Cqrs.Cli.Factories
{
    public interface IParameterValueFactory
    {
        bool Qualifies(Type targetType);
        object Create(Type targetType, object parameterValue);
    }
}
