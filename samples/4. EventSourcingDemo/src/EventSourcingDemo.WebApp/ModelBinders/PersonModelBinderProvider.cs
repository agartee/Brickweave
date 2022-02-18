using EventSourcingDemo.Domain.People.Commands;
using EventSourcingDemo.WebApp.ModelBinders.People;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventSourcingDemo.WebApp.ModelBinders
{
    public class PersonModelBinderProvider : IModelBinderProvider
    {
        private readonly IDictionary<Type, Func<IModelBinder>> _binders = new Dictionary<Type, Func<IModelBinder>>
        {
            [typeof(CreatePerson)] = () => new CreatePersonModelBinder(),
            [typeof(UpdatePerson)] = () => new UpdatePersonModelBinder()
        };

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (_binders.TryGetValue(context.Metadata.ModelType, out var createModelBinderFunc))
                return createModelBinderFunc();

            return null;
        }
    }
}
