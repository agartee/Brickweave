using System;
using Brickweave.Cqrs.SqlServer.Entities;
using Brickweave.Cqrs.SqlServer.Services;
using Brickweave.Cqrs.Services;
using Brickweave.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Brickweave.Cqrs.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brickweave.Cqrs.SqlServer.DependencyInjection
{
    public static class CqrsOptionsBuilderExtensions
    {
        private static bool _longRunningCommandsEnabled;
        private static bool _longRunningCommandCleanupEnabled;

        public static CqrsOptionsBuilder EnableLongRunningCommands<TDbContext>(this CqrsOptionsBuilder builder,
            Func<TDbContext, DbSet<CommandQueueData>> getCommandQueueDbSet,
            TimeSpan pollingInterval) where TDbContext : DbContext
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
                .AddScoped((Func<IServiceProvider, ILongRunningCommandProcessor>)(s => new LongRunningCommandProcessor(
                    s.GetService<ICommandQueue>(),
                    s.GetService<IEnqueuedCommandDispatcher>(),
                    pollingInterval,
                    s.GetService<ILogger<LongRunningCommandProcessor>>())))
                .AddScoped<IEnqueuedCommandDispatcher, CommandDispatcher>()
                .Replace(new ServiceDescriptor(
                    typeof(ICommandQueue), s => Activator.CreateInstance(typeof(SqlServerCommandQueue<>).MakeGenericType(typeof(TDbContext)),
                        s.GetService<TDbContext>(),
                        getCommandQueueDbSet,
                        s.GetService<IDocumentSerializer>()), ServiceLifetime.Scoped));

            return builder;
        }

        public static CqrsOptionsBuilder EnableCommandCleanup(this CqrsOptionsBuilder builder, TimeSpan pollingInterval, TimeSpan deleteCommandsAfter)
        {
            if (_longRunningCommandCleanupEnabled)
                throw new InvalidOperationException("Long-running command cleanup is already enabled.");

            _longRunningCommandCleanupEnabled = true;

            builder.Services()
                .AddScoped((Func<IServiceProvider, ILongRunningCommandCostodian>)(s => new LongRunningCommandCostodian(
                    s.GetService<ICommandQueue>(),
                    pollingInterval, 
                    deleteCommandsAfter,
                    s.GetService<ILogger<LongRunningCommandCostodian>>())));

            return builder;
        }
    }
}
