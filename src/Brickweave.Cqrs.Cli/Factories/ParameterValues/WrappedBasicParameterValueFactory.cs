﻿using System;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories.ParameterValues
{
    public class WrappedBasicParameterValueFactory : ISingleParameterValueFactory
    {
        public bool Qualifies(Type targetType)
        {
            var result = !targetType.IsBasicType() 
                   && !targetType.IsGuidType() 
                   && !targetType.IsDateTimeType() 
                   && !targetType.IsDictionary()
                   && GetWrappedBasicType(targetType) != null;

            return result;
        }

        public object Create(Type targetType, ExecutableParameterInfo parameter)
        {
            if (parameter == null)
                return null;

            var constructor = GetWrappedBasicConstructor(targetType);
            var parameterType = GetWrappedBasicType(targetType);

            return constructor.Invoke(new [] { Convert.ChangeType(parameter.SingleValue, parameterType) });
        }

        private static ConstructorInfo GetWrappedBasicConstructor(Type type)
        {
            return type.GetConstructors()
                .Where(c => c.GetParameters().Length == 1)
                .FirstOrDefault(c => c.GetParameters().First().ParameterType.IsBasicType());
        }

        private static Type GetWrappedBasicType(Type type)
        {
            return GetWrappedBasicConstructor(type)?.GetParameters()
                .First().ParameterType;
        }
    }
}
