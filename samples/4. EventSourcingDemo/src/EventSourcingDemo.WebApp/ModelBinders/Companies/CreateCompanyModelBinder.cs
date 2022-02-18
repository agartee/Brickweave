using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Companies.Commands;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventSourcingDemo.WebApp.ModelBinders.Companies
{
    public class CreateCompanyModelBinder : IModelBinder
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
                var model = new CreateCompany(new Name(name.FirstValue));
                bindingContext.Result = ModelBindingResult.Success(model);
            }

            return Task.CompletedTask;
        }
    }
}
