using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LiteGuard;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs
{
    public class ProjectionDispatcher : IProjectionDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public ProjectionDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync(ICommand command)
        {
            Guard.AgainstNullArgument(nameof(command), command);

            var handlers = GetProjectionHandlers(command);

            foreach (dynamic handler in handlers)
            {
                await handler.HandleAsync((dynamic)command);
            }
        }

        private IEnumerable<object> GetProjectionHandlers(ICommand command)
        {
            return _serviceProvider.GetServices(
                typeof(IProjectionHandler<>).MakeGenericType(command.GetType()));
        }
    }
}
