using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Extensions;
using LiteGuard;
using Microsoft.Extensions.Logging;

namespace Brickweave.Cqrs.Services
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<QueryDispatcher> _logger;

        public QueryDispatcher(IServiceProvider serviceProvider, ILogger<QueryDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<object> ExecuteAsync(IQuery query, ClaimsPrincipal user = null)
        {
            Guard.AgainstNullArgument(nameof(query), query);

            dynamic handler = GetQueryHandler(query, query.GetQueryReturnType());

            _logger.LogInformation($"{ query.GetType() } query will be handled by { handler.GetType() }.");

            if (handler is ISecured)
                return await handler.HandleAsync((dynamic)query, user);

            return await handler.HandleAsync((dynamic)query);
        }

        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, ClaimsPrincipal user = null)
        {
            Guard.AgainstNullArgument(nameof(query), query);

            dynamic handler = GetQueryHandler(query, typeof(TResult));

            _logger.LogInformation($"{ query.GetType() } query will be handled by { handler.GetType() }.");

            if (handler is ISecured)
                return await handler.HandleAsync((dynamic)query, user);

            return await handler.HandleAsync((dynamic)query);
        }

        private object GetQueryHandler(IQuery query, Type queryReturnType)
        {
            foreach (var handlerType in GetPossibleHandlerTypes(query, queryReturnType))
            {
                var result = _serviceProvider.GetService(handlerType);
                if (result != null)
                    return result;
            }

            throw new QueryHandlerNotRegisteredException(query);
        }

        private IEnumerable<Type> GetPossibleHandlerTypes(IQuery query, Type queryReturnType)
        {
            return new[]
            {
                    typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), queryReturnType),
                    typeof(ISecuredQueryHandler<,>).MakeGenericType(query.GetType(), queryReturnType)
                };
        }
    }
}
