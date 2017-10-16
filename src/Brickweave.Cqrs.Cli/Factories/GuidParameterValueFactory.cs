using System;
using Brickweave.Cqrs.Cli.Extensions;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class GuidParameterValueFactory : IParameterValueFactory
    {
        public bool Qualifies(Type targetType)
        {
            return targetType.IsGuidType();
        }

        public object Create(Type targetType, object parameterValue)
        {
            if(parameterValue.GetType() != typeof(string))
                throw new InvalidOperationException($"Unsupported type for {typeof(IExecutable)}:{targetType} parameter conversion: {parameterValue.GetType()}");

            return new Guid((string) parameterValue);
        }
    }
}