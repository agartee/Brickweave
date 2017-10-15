using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Extensions;

namespace Brickweave.Cqrs
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryExecutor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<object> ExecuteAsync(IQuery query)
        {
            Guard.IsNotNullQuery(query);

            var queryReturnType = query.GetQueryReturnType();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), queryReturnType);
            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new QueryHandlerNotRegisteredException(query);

            return await handler.HandleAsync((dynamic)query);
        }

        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query)
        {
            Guard.IsNotNullQuery(query);

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new QueryHandlerNotRegisteredException(query);

            return await handler.HandleAsync((dynamic)query);
        }

        private static class Guard
        {
            public static void IsNotNullQuery(IQuery query)
            {
                if (query == null)
                    throw new ArgumentNullException($"{typeof(QueryExecutor)} cannot process a null query.");
            }
        }
    }
}