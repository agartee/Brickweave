using System;
using AdvancedCqrs.Domain.Things.Commands;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdvancedCqrs.WebApp.ModelBinders
{
    public class ThingCommandModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(CreateThing))
                return new CreateThingModelBinder();

            if (context.Metadata.ModelType == typeof(UpdateThing))
                return new UpdateThingModelBinder();

            return null;
        }
    }
}
