using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Models;
using LiteGuard;

namespace Brickweave.Cqrs.Cli.Readers
{
    public class XmlDocumentationFileConstructorHelpReader : IExecutableHelpReader
    {
        private readonly string[] _filePaths;
        private readonly IEnumerable<IExecutableRegistration> _executableRegistrations;
        private readonly IEnumerable<Type> _excludedExecutableTypes = new List<Type>();

        public XmlDocumentationFileConstructorHelpReader(params string[] filePaths) 
            : this(Enumerable.Empty<IExecutableRegistration>(), Enumerable.Empty<Type>(), filePaths)
        {
        }
                
        public XmlDocumentationFileConstructorHelpReader(IEnumerable<IExecutableRegistration> executableRegistrations,
            IEnumerable<Type> excludedExecutableTypes, params string[] filePaths)
        {
            _executableRegistrations = executableRegistrations;
            _excludedExecutableTypes = excludedExecutableTypes;
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
                throw new ExecutableHelpFileNotFoundExeption(filePath);

            try
            {
                var document = XDocument.Load(filePath);
                var results = ReadConstructorSummary(document, adjacencyCriteria);

                return results;
            }
            catch { throw new ExecutableHelpFileInvalidExeption(); }
        }

        private HelpInfo[] ReadConstructorSummary(XDocument document, HelpAdjacencyCriteria adjacencyCriteria)
        {
            var results = document.Root.Element("members").Elements("member")
                    .Where(IsConstructorWithSummary)
                    .Where(IsNotInExcludedTypes)
                    .Select(CreateHelpInfo)
                    .Where(h => adjacencyCriteria.Subject == h.Subject)
                    .Where(h => string.IsNullOrWhiteSpace(adjacencyCriteria.Action)
                        || adjacencyCriteria.Action == h.Name)
                    .ToArray();

            return results;
        }

        private bool IsConstructorWithSummary(XElement element)
        {
            return element.Attribute("name").Value.Contains("#ctor");
        }
        
        private bool IsNotInExcludedTypes(XElement element)
        {
            var typeName = GetTypeFullName(element);

            return !_excludedExecutableTypes
                .Select(t => t.FullName)
                .Any(n => typeName == n);
        }

        private HelpInfo CreateHelpInfo(XElement constructorElement)
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

        private string GetSubjectName(string typeName)
        {
            var registered = _executableRegistrations
                .FirstOrDefault(r => r.Type.Name == typeName);

            return registered != null
                ? registered.SubjectName
                : string.Join(" ", SplitTypeName(typeName).Skip(1));
        }

        private string GetActionName(string typeName)
        {
            var registered = _executableRegistrations
                .FirstOrDefault(r => r.Type.Name == typeName);

            return registered != null
                ? registered.ActionName
                : SplitTypeName(typeName).First();
        }

        private string GetTypeFullName(XElement constructorElement)
        {
            var name = constructorElement.Attribute("name").Value;
            var start = name.IndexOf("M:") + "M:".Length;
            var end = name.Substring(start).IndexOf(".#ctor");

            return name.Substring(start, end);
        }

        private string GetTypeName(XElement constructorElement)
        {
            return constructorElement.Attribute("name").Value
                .Split(new[] { "#ctor" }, StringSplitOptions.RemoveEmptyEntries)
                .First().Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries)
                .Last();
        }

        private string[] SplitTypeName(string typeName)
        {
            return Regex.Replace(typeName, "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1")
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToLower(CultureInfo.InvariantCulture))
                .ToArray();
        }
    }
}
