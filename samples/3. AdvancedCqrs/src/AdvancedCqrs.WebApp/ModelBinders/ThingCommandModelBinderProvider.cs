using System;
using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Commands;
using AdvancedCqrs.Domain.Things.Models;
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

    public class CreateThingModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var name = bindingContext.ValueProvider.GetValue("name");
            if (name == ValueProviderResult.None || string.IsNullOrEmpty(name.FirstValue))
            {
                bindingContext.ModelState.TryAddModelError("name", $"Missing { nameof(name) } parameter.");
            }

            var model = new CreateThing(name.FirstValue);

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }

    public class UpdateThingModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var id = bindingContext.ValueProvider.GetValue("id");
            if (id == ValueProviderResult.None || string.IsNullOrEmpty(id.FirstValue))
            {
                bindingContext.ModelState.TryAddModelError("name", $"Missing { nameof(id) } parameter.");
            }

            var name = bindingContext.ValueProvider.GetValue("name");
            if (name == ValueProviderResult.None || string.IsNullOrEmpty(name.FirstValue))
            {
                bindingContext.ModelState.TryAddModelError("name", $"Missing { nameof(name) } parameter.");
            }

            var model = new UpdateThing(new ThingId(new Guid(id.FirstValue)), name.FirstValue);

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
