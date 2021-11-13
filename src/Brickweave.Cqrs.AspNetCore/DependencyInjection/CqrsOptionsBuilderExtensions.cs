using Brickweave.Cqrs.AspNetCore.BackgroundServices;
using Brickweave.Cqrs.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.AspNetCore.DependencyInjection
{
    public static class CqrsOptionsBuilderExtensions
    {
        public static CqrsOptionsBuilder AddLongRunningCommandBackgroundService(this CqrsOptionsBuilder builder)
        {
            builder.Services()
                .AddHostedService<LongRunningCommandBackgroundService>();

            return builder;
        }

        public static CqrsOptionsBuilder AddLongRunningCommandCustodianBackgroundService(this CqrsOptionsBuilder builder)
        {
            builder.Services()
                .AddHostedService<LongRunningCommandCustodianBackgroundService>();

            return builder;
        }
    }
}
