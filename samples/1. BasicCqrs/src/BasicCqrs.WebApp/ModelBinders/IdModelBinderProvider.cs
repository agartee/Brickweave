using System;
using System.Collections.Generic;
using BasicCqrs.Domain.People.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace BasicCqrs.WebApp.ModelBinders
{
    public class IdModelBinderProvider : IModelBinderProvider
    {
        private static readonly IEnumerable<Type> SupportedTypes = new List<Type>
        {
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(ulong),
            typeof(decimal),
            typeof(float),
            typeof(double),
            typeof(char),
            typeof(Guid),
            typeof(string)
        };

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(PersonId))
            {
                return new BinderTypeModelBinder(typeof(IdModelBinder<PersonId>));
            }

            return null;
        }
    }
}
