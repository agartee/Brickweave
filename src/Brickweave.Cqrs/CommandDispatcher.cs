﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Extensions;
using LiteGuard;

namespace Brickweave.Cqrs
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<object> ExecuteAsync(ICommand command, ClaimsPrincipal user = null)
        {
            Guard.AgainstNullArgument(nameof(command), command);

            dynamic handler = GetCommandHandler(command, command.GetCommandReturnType());
            
            if (command.GetCommandReturnType() != null)
            {
                if(handler is ISecured)
                    return await handler.HandleAsync((dynamic)command, user);

                return await handler.HandleAsync((dynamic)command);
            }

            if (handler is ISecured)
                await handler.HandleAsync((dynamic)command, user);
            else
                await handler.HandleAsync((dynamic)command);

            return null;
        }

        public async Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, ClaimsPrincipal user = null)
        {
            Guard.AgainstNullArgument(nameof(command), command);

            dynamic handler = GetCommandHandler(command, typeof(TResult));

            if (handler is ISecured)
                return await handler.HandleAsync((dynamic)command, user);

            return await handler.HandleAsync((dynamic)command);
        }

        private object GetCommandHandler(ICommand command, Type commandReturnType)
        {
            foreach (var handlerType in GetPossibleHandlerTypes(command, commandReturnType))
            {
                var result = _serviceProvider.GetService(handlerType);
                if (result != null)
                    return result;
            }

            throw new CommandHandlerNotRegisteredException(command);
        }

        private IEnumerable<Type> GetPossibleHandlerTypes(ICommand command, Type commandReturnType)
        {
            return commandReturnType != null
                ? new[]
                {
                        typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), commandReturnType),
                        typeof(ISecuredCommandHandler<,>).MakeGenericType(command.GetType(), commandReturnType)
                }
                : new[]
                {
                        typeof(ICommandHandler<>).MakeGenericType(command.GetType()),
                        typeof(ISecuredCommandHandler<>).MakeGenericType(command.GetType())
                };
        }
    }
}
