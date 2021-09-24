using System;
using BasicCqrs.Domain.People.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace BasicCqrs.WebApp.ModelBinders
{
    public class IdModelBinderProvider : IModelBinderProvider
    {
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
