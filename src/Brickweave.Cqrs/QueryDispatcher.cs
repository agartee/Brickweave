using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Extensions;
using LiteGuard;

namespace Brickweave.Cqrs
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<object> ExecuteAsync(IQuery query, ClaimsPrincipal user = null)
        {
            Guard.AgainstNullArgument(nameof(query), query);

            dynamic handler = GetQueryHandler(query, query.GetQueryReturnType());

            if (handler is ISecured)
                return await handler.HandleAsync((dynamic)query, user);

            return await handler.HandleAsync((dynamic)query);
        }

        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, ClaimsPrincipal user = null)
        {
            Guard.AgainstNullArgument(nameof(query), query);

            dynamic handler = GetQueryHandler(query, typeof(TResult));

            if (handler is ISecured)
                return await handler.HandleAsync((dynamic)query, user);

            return await handler.HandleAsync((dynamic)query);
        }

        private object GetQueryHandler(IQuery query, Type queryReturnType)
        {
            foreach (var handlerType in GetPossibleHandlerTypes())
            {
                var result = _serviceProvider.GetService(handlerType);
                if (result != null)
                    return result;
            }

            throw new QueryHandlerNotRegisteredException(query);

            IEnumerable<Type> GetPossibleHandlerTypes()
            {
                return new[]
                {
                    typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), queryReturnType),
                    typeof(ISecuredQueryHandler<,>).MakeGenericType(query.GetType(), queryReturnType)
                };
            }
        }
    }
}