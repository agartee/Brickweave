using EventSourcingDemo.Domain.Companies.Commands;
using EventSourcingDemo.WebApp.ModelBinders.Companies;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EventSourcingDemo.WebApp.ModelBinders
{
    public class CompanyModelBinderProvider : IModelBinderProvider
    {
        private readonly IDictionary<Type, Func<IModelBinder>> _binders = new Dictionary<Type, Func<IModelBinder>>
        {
            [typeof(CreateCompany)] = () => new CreateCompanyModelBinder(),
            [typeof(UpdateCompany)] = () => new UpdateCompanyModelBinder()
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
