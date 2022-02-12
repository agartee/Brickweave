using System;
using System.Collections.Generic;
using AdvancedCqrs.Domain.Things.Commands;
using AdvancedCqrs.WebApp.ModelBinders.Things;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AdvancedCqrs.WebApp.ModelBinders
{
    public class ThingModelBinderProvider : IModelBinderProvider
    {
        private readonly IDictionary<Type, Func<IModelBinder>> _binders = new Dictionary<Type, Func<IModelBinder>>
        {
            // tip: BinderTypeModelBinder allows for DI into the ModelBinder
            [typeof(CreateThing)] = () => new BinderTypeModelBinder(typeof(CreateThingModelBinder)),
            [typeof(UpdateThing)] = () => new UpdateThingModelBinder()
        };

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if(_binders.TryGetValue(context.Metadata.ModelType, out var createModelBinderFunc))
                return createModelBinderFunc();

            return null;
        }
    }
}
