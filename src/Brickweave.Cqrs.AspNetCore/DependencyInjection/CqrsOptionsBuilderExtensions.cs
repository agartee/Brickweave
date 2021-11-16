using System;
using Brickweave.Cqrs.AspNetCore.BackgroundServices;
using Brickweave.Cqrs.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.AspNetCore.DependencyInjection
{
    public static class CqrsOptionsBuilderExtensions
    {
        public static CqrsOptionsBuilder AddLongRunningCommandBackgroundService(this CqrsOptionsBuilder builder, TimeSpan pollingInterval)
        {
            builder.Services()
                .AddHostedService<LongRunningCommandBackgroundService>()
                .AddSingleton(new LongRunningCommandBackgroundServiceConfig(pollingInterval));

            return builder;
        }

        public static CqrsOptionsBuilder AddLongRunningCommandCustodianBackgroundService(this CqrsOptionsBuilder builder, TimeSpan pollingInterval)
        {
            builder.Services()
                .AddHostedService<LongRunningCommandCustodianBackgroundService>()
                .AddSingleton(new LongRunningCommandCustodianBackgroundServiceConfig(pollingInterval));

            return builder;
        }
    }
}
