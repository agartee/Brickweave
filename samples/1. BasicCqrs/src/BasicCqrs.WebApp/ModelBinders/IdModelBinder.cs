using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BasicCqrs.WebApp.ModelBinders
{
    public class IdModelBinder : IModelBinder
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
                var constructor = GetConstructor(bindingContext.ModelType);

                var currentType = value.GetType();
                var targetType = constructor.GetParameters().First().ParameterType;

                object result = null;
                if (currentType == targetType)
                    result = constructor.Invoke(new[] { value });

                if (targetType == typeof(Guid))
                    result = constructor.Invoke(new object[] { new Guid(value) });

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
