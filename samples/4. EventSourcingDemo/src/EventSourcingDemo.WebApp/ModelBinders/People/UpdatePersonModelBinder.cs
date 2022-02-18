using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.People.Commands;
using EventSourcingDemo.Domain.People.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventSourcingDemo.WebApp.ModelBinders.People
{
    public class UpdatePersonModelBinder : IModelBinder
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

            if(bindingContext.ModelState.IsValid)
            {
                var model = new UpdatePerson(new PersonId(new Guid(id.FirstValue)), new Name(name.FirstValue));
                bindingContext.Result = ModelBindingResult.Success(model);
            }

            return Task.CompletedTask;
        } 
    }
}
