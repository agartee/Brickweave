using System;
using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Commands;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AdvancedCqrs.WebApp.ModelBinders.Things
{
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
}
