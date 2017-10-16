using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.Cli.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCli(
            this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddScoped<IArgParser, NamingConventionArgParser>();

            var executables = assemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => typeof(IExecutable).IsAssignableFrom(t.GetTypeInfo()))
                .ToList();
            
            services.AddScoped<IParameterValueFactory, BasicParameterValueFactory>();
            services.AddScoped<IParameterValueFactory, WrappedBasicParameterValueFactory>();
            services.AddScoped<IParameterValueFactory, GuidParameterValueFactory>();
            services.AddScoped<IParameterValueFactory, WrappedGuidParameterValueFactory>();

            return services.AddScoped<IExecutableFactory>(provider => new ExecutableFactory(
                provider.GetServices<IParameterValueFactory>(),
                executables));
        }
    }
}
