using System;
using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Commands;
using AdvancedCqrs.Domain.Things.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdvancedCqrs.WebApp.ModelBinders
{
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
