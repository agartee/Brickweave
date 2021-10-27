using System;
using Brickweave.Cqrs.Cli.DependencyInjection;
using Brickweave.Cqrs.SqlServer.Entities;
using Brickweave.Cqrs.SqlServer.Services;
using Brickweave.Cqrs.Services;
using Brickweave.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.SqlServer.DependencyInjection
{
    public static class CliOptionsBuilderExtensions
    {
        private static bool _longRunningCommandsEnabled;

        public static CliOptionsBuilder EnableLongRunningCommands<TDbContext>(this CliOptionsBuilder builder,
            Func<TDbContext, DbSet<CommandQueueData>> getCommandQueueDbSet,
            Func<TDbContext, DbSet<CommandStatusData>> getCommandStatusDbSet,
            int pollingIntervalInSeconds) where TDbContext : DbContext
        {
            if (_longRunningCommandsEnabled)
                throw new InvalidOperationException("Long-running commands are already enabled.");

            builder.Services()
                .AddScoped(s => (ICommandQueue) Activator.CreateInstance(
                    typeof(SqlServerCommandQueue<>).MakeGenericType(typeof(TDbContext)), 
                    s.GetService<TDbContext>(),
                    getCommandQueueDbSet,
                    s.GetService<IDocumentSerializer>()))
                .AddScoped(s => (ICommandStatusRepository) Activator.CreateInstance(
                    typeof(SqlServerCommandStatusRepository<>).MakeGenericType(typeof(TDbContext)),
                    s.GetService<TDbContext>(),
                    getCommandQueueDbSet,
                    getCommandStatusDbSet,
                    s.GetService<IDocumentSerializer>()))
                .AddScoped<ICommandProcessor>(s => new CommandProcessor(
                    s.GetService<ICommandQueue>(),
                    s.GetService<ICommandStatusRepository>(),
                    s.GetService<IDispatcher>(),
                    pollingIntervalInSeconds));

            return builder;
        }
    }
}
