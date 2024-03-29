﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.Cli.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static CliOptionsBuilder AddBrickweaveCli(
            this IServiceCollection services, params Assembly[] domainAssemblies)
        {
            return new CliOptionsBuilder(services, domainAssemblies);
        }
    }
}
