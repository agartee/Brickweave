using System;
using Brickweave.Cqrs.SqlServer.Entities;
using Brickweave.Cqrs.SqlServer.Services;
using Brickweave.Cqrs.Services;
using Brickweave.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Brickweave.Cqrs.DependencyInjection;

namespace Brickweave.Cqrs.SqlServer.DependencyInjection
{
    public static class CqrsOptionsBuilderExtensions
    {
        private static bool _longRunningCommandsEnabled;

        public static CqrsOptionsBuilder EnableLongRunningCommands<TDbContext>(this CqrsOptionsBuilder builder,
            Func<TDbContext, DbSet<CommandQueueData>> getCommandQueueDbSet,
            int pollingIntervalInSeconds) where TDbContext : DbContext
        {
            if (_longRunningCommandsEnabled)
                throw new InvalidOperationException("Long-running commands are already enabled.");

            _longRunningCommandsEnabled = true;

            builder.Services()
                .AddScoped(s => (ICommandStatusProvider)Activator.CreateInstance(
                    typeof(SqlServerCommandStatusProvider<>).MakeGenericType(typeof(TDbContext)),
                    s.GetService<TDbContext>(),
                    getCommandQueueDbSet,
                    s.GetService<IDocumentSerializer>()))
                .AddScoped((Func<IServiceProvider, ICommandProcessor>)(s => new CommandProcessor(
                    s.GetService<ICommandQueue>(),
                    s.GetService<IEnqueuedCommandDispatcher>(),
                    pollingIntervalInSeconds)))
                .AddScoped<IEnqueuedCommandDispatcher, CommandDispatcher>()
                .Replace(new ServiceDescriptor(
                    typeof(ICommandQueue), s => Activator.CreateInstance(typeof(SqlServerCommandQueue<>).MakeGenericType(typeof(TDbContext)),
                        s.GetService<TDbContext>(),
                        getCommandQueueDbSet,
                        s.GetService<IDocumentSerializer>()), ServiceLifetime.Scoped));

            return builder;
        }
    }
}
