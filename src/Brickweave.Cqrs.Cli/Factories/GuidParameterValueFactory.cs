using System;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class GuidParameterValueFactory : ISingleParameterValueFactory
    {
        public bool Qualifies(Type targetType)
        {
            return targetType.IsGuidType();
        }

        public object Create(Type targetType, ExecutableParameterInfo parameter)
        {
            return new Guid(parameter.SingleValue);
        }
    }
}