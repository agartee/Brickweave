using System.IO;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Readers;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.Cli.DependencyInjection
{
    public class CliOptionsBuilder
    {
        private readonly IServiceCollection _services;

        public CliOptionsBuilder(IServiceCollection services, params Assembly[] domainAssemblies)
        {
            var executables = domainAssemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => typeof(IExecutable).IsAssignableFrom(t.GetTypeInfo()))
                .ToList();

            services
                .AddScoped<IRunner, Runner>()
                .AddScoped<IExecutableInfoFactory, NamingConventionExecutableInfoFactory>()
                .AddScoped<IHelpInfoFactory, HelpInfoFactory>()
                .AddScoped<IParameterValueFactory, BasicParameterValueFactory>()
                .AddScoped<IParameterValueFactory, WrappedBasicParameterValueFactory>()
                .AddScoped<IParameterValueFactory, GuidParameterValueFactory>()
                .AddScoped<IParameterValueFactory, WrappedGuidParameterValueFactory>()
                .AddScoped<IExecutableFactory>(provider => new ExecutableFactory(
                    provider.GetServices<IParameterValueFactory>(),
                    executables))
                .AddScoped<IExecutableHelpReader>(s => new NamingConventionXmlDocumentationFileHelpReader(domainAssemblies
                    .Select(a => Path.Combine(
                        Path.GetDirectoryName(a.Location), 
                        $"{a.GetName().Name}.xml"))
                    .ToArray()));

            _services = services;
        }

        public CliOptionsBuilder AddCategoryHelpFile(string filePath)
        {
            _services.AddScoped<ICategoryHelpReader>(services => new JsonFileCategoryHelpReader(filePath));
            return this;
        }
        
        public IServiceCollection Services()
        {
            return _services;
        }
    }
}
