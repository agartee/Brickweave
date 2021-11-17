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
            Func<TDbContext, DbSet<CommandQueueData>> getCommandQueueDbSet, Func<TDbContext, DbSet<CommandStatusData>> getCommandStatusDbSet) 
            where TDbContext : DbContext
        {
            if (_longRunningCommandsEnabled)
                throw new InvalidOperationException("Long-running commands are already enabled.");

            _longRunningCommandsEnabled = true;

            builder.Services()
                .AddScoped(s => (ICommandStatusProvider)Activator.CreateInstance(
                    typeof(SqlServerCommandStatusProvider<>).MakeGenericType(typeof(TDbContext)),
                    s.GetService<TDbContext>(),
                    getCommandStatusDbSet,
                    s.GetService<IDocumentSerializer>()))
                .AddScoped((Func<IServiceProvider, ILongRunningCommandProcessor>)(s => new LongRunningCommandProcessor(
                    s.GetService<ICommandQueue>(),
                    s.GetService<IEnqueuedCommandDispatcher>(),
                    s.GetService<ILogger<LongRunningCommandProcessor>>())))
                .AddScoped<IEnqueuedCommandDispatcher, CommandDispatcher>()
                .Replace(new ServiceDescriptor(
                    typeof(ICommandQueue), s => Activator.CreateInstance(typeof(SqlServerCommandQueue<>).MakeGenericType(typeof(TDbContext)),
                        s.GetService<IDbContextFactory<TDbContext>>(),
                        getCommandQueueDbSet,
                        getCommandStatusDbSet,
                        s.GetService<IDocumentSerializer>()), ServiceLifetime.Singleton));

            return builder;
        }

        public static CqrsOptionsBuilder EnableCommandCleanup(this CqrsOptionsBuilder builder, TimeSpan deleteCommandsAfter)
        {
            if (_longRunningCommandCleanupEnabled)
                throw new InvalidOperationException("Long-running command cleanup is already enabled.");

            _longRunningCommandCleanupEnabled = true;

            builder.Services()
                .AddSingleton((Func<IServiceProvider, ILongRunningCommandCostodian>)(s => new LongRunningCommandCostodian(
                    s.GetService<ICommandQueue>(),
                    deleteCommandsAfter,
                    s.GetService<ILogger<LongRunningCommandCostodian>>())));

            return builder;
        }
    }
}
