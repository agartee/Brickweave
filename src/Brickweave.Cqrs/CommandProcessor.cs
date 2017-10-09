using System;
using System.Threading.Tasks;
using Brickweave.Core.Exceptions;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Extensions;

namespace Brickweave.Cqrs
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<object> ProcessAsync(ICommand command)
        {
            Guard.IsNotNullCommand(command);

            var commandReturnType = command.GetCommandReturnType();

            var handlerType = commandReturnType != null
                ? typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), commandReturnType)
                : typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new CommandHandlerNotRegisteredException(command);

            if (commandReturnType != null)
                return await handler.HandleAsync((dynamic)command);

            await handler.HandleAsync((dynamic)command);
            return null;
        }

        public async Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command)
        {
            Guard.IsNotNullCommand(command);

            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            dynamic handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
                throw new CommandHandlerNotRegisteredException(command);

            return await handler.HandleAsync((dynamic)command);
        }

        private static class Guard
        {
            public static void IsNotNullCommand(ICommand command)
            {
                if (command == null)
                    throw new GuardException("Unable to process null command.");
            }
        }
    }
}
