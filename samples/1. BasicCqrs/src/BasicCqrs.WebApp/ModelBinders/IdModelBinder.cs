using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Brickweave.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BasicCqrs.WebApp.ModelBinders
{
    public class IdModelBinder<T> : IModelBinder where T : Id<Guid>
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var fieldName = bindingContext.FieldName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(fieldName);
            
            if (valueProviderResult == ValueProviderResult.None) 
                return Task.CompletedTask;
            else 
                bindingContext.ModelState.SetModelValue(fieldName, valueProviderResult);

            string value = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(value)) 
                return Task.CompletedTask;

            try
            {
                var constructor = GetConstructor(typeof(T));
                var result = constructor.Invoke(new object[] { new Guid(value) });

                bindingContext.Result = ModelBindingResult.Success(result);
            }
            catch (Exception)
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }

            return Task.CompletedTask;
        }

        private static ConstructorInfo GetConstructor(Type type)
        {
            return type.GetConstructors()
                .Where(c => c.GetParameters().Length == 1)
                .FirstOrDefault();
        }
    }
}
