using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Models;
using LiteGuard;

namespace Brickweave.Cqrs.Cli.Readers
{
    public class XmlDocumentationFileHelpReader : IExecutableHelpReader
    {
        private readonly string[] _filePaths;
        private readonly IEnumerable<IExecutableRegistration> _executableRegistrations;

        public XmlDocumentationFileHelpReader(params string[] filePaths) 
            : this(Enumerable.Empty<IExecutableRegistration>(), filePaths)
        {
        }

        public XmlDocumentationFileHelpReader(IEnumerable<IExecutableRegistration> executableRegistrations, params string[] filePaths)
        {
            _executableRegistrations = executableRegistrations;
            _filePaths = filePaths;
        }
        
        public IEnumerable<HelpInfo> GetHelpInfo(HelpAdjacencyCriteria adjacencyCriteria)
        {
            return _filePaths
                .SelectMany(filePath => Read(filePath, adjacencyCriteria))
                .ToList();
        }

        private IEnumerable<HelpInfo> Read(string filePath, HelpAdjacencyCriteria adjacencyCriteria)
        {
            Guard.AgainstNullArgument(nameof(adjacencyCriteria), adjacencyCriteria);

            if (!File.Exists(filePath))
                throw new ExecutableHelpFileNotFoundExeption();

            try
            {
                var document = XDocument.Load(filePath);

                var results = document.Root.Element("members").Elements("member")
                    .Where(IsConstructorWithDocumentation)
                    .Select(CreateHelpInfo)
                    .Where(h => adjacencyCriteria.Subject == h.Subject)
                    .Where(h => string.IsNullOrWhiteSpace(adjacencyCriteria.Action) 
                        || adjacencyCriteria.Action == h.Name)
                    .ToArray();

                return results;
            }
            catch { throw new ExecutableHelpFileInvalidExeption(); }

            bool IsConstructorWithDocumentation(XElement element)
            {
                return element.Attribute("name").Value.Contains("#ctor");
            }

            HelpInfo CreateHelpInfo(XElement constructorElement)
            {
                var typeName = GetTypeName(constructorElement);

                var subjectName = GetSubjectName(typeName);
                var actionName = GetActionName(typeName);

                return new HelpInfo(
                    actionName,
                    subjectName,
                    constructorElement.Element("summary")?.Value.Trim(),
                    HelpInfoType.Executable,
                    constructorElement.Elements("param")
                        .Select(p => new HelpInfo(
                            $"{p.Attribute("name")?.Value}",
                            $"{subjectName} {actionName}",
                            p.Value,
                            HelpInfoType.Parameter))
                        .ToArray());
            }

            string GetSubjectName(string typeName)
            {
                var registered = _executableRegistrations
                    .FirstOrDefault(r => r.Type.Name == typeName);

                return registered != null
                    ? registered.SubjectName
                    : string.Join(" ", SplitTypeName(typeName).Skip(1));
            }

            string GetActionName(string typeName)
            {
                var registered = _executableRegistrations
                    .FirstOrDefault(r => r.Type.Name == typeName);

                return registered != null
                    ? registered.ActionName
                    : SplitTypeName(typeName).First();
            }

            string GetTypeName(XElement constructorElement)
            {
                return constructorElement.Attribute("name").Value
                    .Split(new[] {"#ctor"}, StringSplitOptions.RemoveEmptyEntries)
                    .First().Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries)
                    .Last();
            }

            string[] SplitTypeName(string typeName)
            {
                return Regex.Replace(typeName, "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1")
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.ToLower(CultureInfo.InvariantCulture))
                    .ToArray();
            }
        }
    }
}
