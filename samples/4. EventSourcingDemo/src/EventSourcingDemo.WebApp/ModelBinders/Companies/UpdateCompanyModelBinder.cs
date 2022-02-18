using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Companies.Commands;
using EventSourcingDemo.Domain.Companies.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventSourcingDemo.WebApp.ModelBinders.Companies
{
    public class UpdateCompanyModelBinder : IModelBinder
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
                var model = new UpdateCompany(new CompanyId(new Guid(id.FirstValue)), new Name(name.FirstValue));
                bindingContext.Result = ModelBindingResult.Success(model);
            }

            return Task.CompletedTask;
        } 
    }
}
