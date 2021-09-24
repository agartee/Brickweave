using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Factories.ParameterValues;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Readers;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Cqrs.Cli.DependencyInjection
{
    public class CliOptionsBuilder
    {
        private readonly IServiceCollection _services;
        private readonly IList<IExecutableRegistration> _executableRegistrations = new List<IExecutableRegistration>();
        private readonly IList<Type> _excludedExecutableTypes = new List<Type>();
        private readonly IEnumerable<string> _xmlDocumentationFilePaths = new List<string>();

        private CultureInfo _culture;

        public CliOptionsBuilder(IServiceCollection services, params Assembly[] domainAssemblies)
        {
            var executables = domainAssemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => typeof(IExecutable).IsAssignableFrom(t.GetTypeInfo()))
                .ToList();

            _xmlDocumentationFilePaths = domainAssemblies
                .Select(a => Path.Combine(Path.GetDirectoryName(a.Location), $"{a.GetName().Name}.xml"))
                .ToList();

            services
                .AddScoped<ICliDispatcher, CliDispatcher>()
                .AddScoped<IExecutableInfoFactory>(provider =>
                    new ExecutableInfoFactory(_executableRegistrations.ToArray()))
                .AddScoped<BasicParameterValueFactory>()
                .AddScoped<WrappedBasicParameterValueFactory>()
                .AddScoped<GuidParameterValueFactory>()
                .AddScoped<WrappedGuidParameterValueFactory>()
                .AddScoped<KeyValuePairParameterValueFactory>()
                .AddScoped<IParameterValueFactory, BasicParameterValueFactory>()
                .AddScoped<IParameterValueFactory, WrappedBasicParameterValueFactory>()
                .AddScoped<IParameterValueFactory, GuidParameterValueFactory>()
                .AddScoped<IParameterValueFactory, WrappedGuidParameterValueFactory>()
                .AddScoped<IParameterValueFactory>(s => new DateTimeParameterValueFactory(_culture))
                .AddScoped<IParameterValueFactory>(s => new KeyValuePairParameterValueFactory(
                    new ISingleParameterValueFactory[] {
                        s.GetService<BasicParameterValueFactory>(),
                        s.GetService<WrappedBasicParameterValueFactory>(),
                        s.GetService<GuidParameterValueFactory>(),
                        s.GetService<WrappedGuidParameterValueFactory>()
                    }))
                .AddScoped<ISingleParameterValueFactory, WrappedBasicParameterValueFactory>()
                .AddScoped<ISingleParameterValueFactory, BasicParameterValueFactory>()
                .AddScoped<ISingleParameterValueFactory, GuidParameterValueFactory>()
                .AddScoped<ISingleParameterValueFactory, WrappedGuidParameterValueFactory>()
                .AddScoped<ISingleParameterValueFactory>(s => new DateTimeParameterValueFactory(_culture))
                .AddScoped<ISingleParameterValueFactory>(s => new KeyValuePairParameterValueFactory(
                    new ISingleParameterValueFactory[] {
                        s.GetService<BasicParameterValueFactory>(),
                        s.GetService<WrappedBasicParameterValueFactory>(),
                        s.GetService<GuidParameterValueFactory>(),
                        s.GetService<WrappedGuidParameterValueFactory>()
                    }))
                .AddScoped<IParameterValueFactory, DictionaryParameterValueFactory>()
                .AddScoped<IParameterValueFactory>(s => new EnumerableParameterValueFactory(s.GetServices<ISingleParameterValueFactory>()))
                .AddScoped<IParameterValueFactory>(s => new ListParameterValueFactory(s.GetServices<ISingleParameterValueFactory>()))
                .AddScoped<IExecutableFactory>(s => new ExecutableFactory(
                    s.GetServices<IParameterValueFactory>(),
                    executables.Where(t => !_excludedExecutableTypes.Contains(t))))
                .AddScoped<IHelpInfoFactory, HelpInfoFactory>();

            _services = services;
        }

        public CliOptionsBuilder OverrideCommandName<T>(string actionName, params string[] subjectNameParts) where T : class, ICommand
        {
            _executableRegistrations.Add(new ExecutableRegistration<T>(actionName, subjectNameParts));
            return this;
        }

        public CliOptionsBuilder OverrideQueryName<T>(string actionName, params string[] subjectNameParts) where T : class, IQuery
        {
            _executableRegistrations.Add(new ExecutableRegistration<T>(actionName, subjectNameParts));
            return this;
        }

        public CliOptionsBuilder OverrideExecutableName<T>(string actionName, params string[] subjectNameParts) where T : class, IExecutable
        {
            _executableRegistrations.Add(new ExecutableRegistration<T>(actionName, subjectNameParts));
            return this;
        }

        public CliOptionsBuilder ExcludeCommand<T>() where T : class, ICommand
        {
            _excludedExecutableTypes.Add(typeof(T));
            return this;
        }

        public CliOptionsBuilder ExcludeQuery<T>() where T : class, IQuery
        {
            _excludedExecutableTypes.Add(typeof(T));
            return this;
        }
        
        public CliOptionsBuilder ExcludeExecutable<T>() where T : class, IExecutable
        {
            _excludedExecutableTypes.Add(typeof(T));
            return this;
        }

        public CliOptionsBuilder AddCategoryHelpFile(string filePath)
        {
            _services.AddScoped<ICategoryHelpReader>(services => new JsonFileCategoryHelpReader(filePath));
            return this;
        }

        public CliOptionsBuilder AddPreferredDocumentationStrategy(HelpDocumentationStrategy helpDocumentationStrategy)
        {
            if (helpDocumentationStrategy == HelpDocumentationStrategy.ClassesAndProperties)
            {
                _services.AddScoped<IExecutableHelpReader>(s => new XmlDocumentationFileClassesAndPropertiesHelpReader(
                    _executableRegistrations, _excludedExecutableTypes,
                    _xmlDocumentationFilePaths.ToArray()));
            }
            else
            {
                _services.AddScoped<IExecutableHelpReader>(s => new XmlDocumentationFileConstructorHelpReader(
                    _executableRegistrations, _excludedExecutableTypes,
                    _xmlDocumentationFilePaths.ToArray()));
            }

            return this;
        }

        public CliOptionsBuilder AddDateParsingCulture(CultureInfo cultureInfo)
        {
            _culture = cultureInfo;
            return this;
        }
        
        public IServiceCollection Services()
        {
            return _services;
        }
    }
}
