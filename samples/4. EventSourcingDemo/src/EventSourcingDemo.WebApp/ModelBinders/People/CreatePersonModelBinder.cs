using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.People.Commands;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventSourcingDemo.WebApp.ModelBinders.People
{
    public class CreatePersonModelBinder : IModelBinder
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

            if (bindingContext.ModelState.IsValid)
            {
                var model = new CreatePerson(new Name(name.FirstValue));
                bindingContext.Result = ModelBindingResult.Success(model);
            }

            return Task.CompletedTask;
        }
    }
}
