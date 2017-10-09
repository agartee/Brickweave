using System.Threading.Tasks;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Extensions;

namespace Brickweave.Cqrs
{
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IServiceLocator _serviceLocator;

        public QueryProcessor(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public async Task<object> ProcessAsync(IQuery query)
        {
            Guard.IsNotNullQuery(query);

            var queryReturnType = query.GetQueryReturnType();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), queryReturnType);
            dynamic handler = _serviceLocator.GetInstance(handlerType);

            if (handler == null)
                throw new QueryHandlerNotRegisteredException(query);

            return await handler.HandleAsync((dynamic)query);
        }

        public async Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
        {
            Guard.IsNotNullQuery(query);

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _serviceLocator.GetInstance(handlerType);

            if (handler == null)
                throw new QueryHandlerNotRegisteredException(query);

            return await handler.HandleAsync((dynamic)query);
        }

        private static class Guard
        {
            public static void IsNotNullQuery(IQuery query)
            {
                if (query == null)
                    throw new GuardException("Unable to process null query.");
            }
        }
    }
}