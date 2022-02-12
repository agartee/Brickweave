using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Brickweave.Domain.AspNetCore.ModelBinders
{
    public class IdModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var fieldName = bindingContext.FieldName;
            var fieldValue = bindingContext.ValueProvider.GetValue(fieldName);

            if (fieldValue == ValueProviderResult.None)
                return Task.CompletedTask;
            else
                bindingContext.ModelState.SetModelValue(fieldName, fieldValue);

            string value = fieldValue.FirstValue;
            if (string.IsNullOrEmpty(value))
                return Task.CompletedTask;

            try
            {
                var constructor = GetConstructor(bindingContext.ModelType);

                var currentIdValueType = value.GetType();
                var targetIdValueType = constructor.GetParameters().First().ParameterType;

                object result = null;
                if (currentIdValueType == targetIdValueType)
                    result = constructor.Invoke(new[] { value });

                if (targetIdValueType == typeof(Guid))
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
