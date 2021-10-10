using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Models;
using LiteGuard;

namespace Brickweave.Cqrs.Cli.Readers
{
    public class XmlDocumentationFileClassesAndPropertiesHelpReader : IExecutableHelpReader
    {
        private readonly string[] _filePaths;
        private readonly IEnumerable<IExecutableRegistration> _executableRegistrations;
        private readonly IEnumerable<Type> _excludedExecutableTypes = new List<Type>();

        public XmlDocumentationFileClassesAndPropertiesHelpReader(params string[] filePaths)
            : this(Enumerable.Empty<IExecutableRegistration>(), Enumerable.Empty<Type>(), filePaths)
        {
        }

        public XmlDocumentationFileClassesAndPropertiesHelpReader(IEnumerable<IExecutableRegistration> executableRegistrations,
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
                var results = ReadClassAndPropertySummary(document, adjacencyCriteria);

                return results;
            }
            catch { throw new ExecutableHelpFileInvalidExeption(); }
        }

        private HelpInfo[] ReadClassAndPropertySummary(XDocument document, HelpAdjacencyCriteria adjacencyCriteria)
        {
            var results = document.Root.Element("members").Elements("member")
                    .Where(x => !x.Attribute("name").Value.Equals("T:System.Runtime.CompilerServices.IsExternalInit"))
                    .Where(x => IsClassWithSummary(x))
                    .Where(IsNotInExcludedTypes)
                    .Select(x => CreateClassHelpInfo(x, GetPropertyElements(document, x)))
                    .Where(h => adjacencyCriteria.Subject == h.Subject)
                    .Where(h => string.IsNullOrWhiteSpace(adjacencyCriteria.Action)
                        || adjacencyCriteria.Action == h.Name)
                    .ToArray();

            return results;
        }

        private bool IsClassWithSummary(XElement element)
        {
            return element.Attribute("name").Value.Contains("T:");
        }

        private bool IsNotInExcludedTypes(XElement element)
        {
            var typeName = GetTypeFullName(element);

            return !_excludedExecutableTypes
                .Select(t => t.FullName)
                .Any(n => typeName == n);
        }

        private IEnumerable<XElement> GetPropertyElements(XDocument document, XElement typeElement)
        {
            var typeName = GetTypeFullName(typeElement);

            var results = document.Root.Element("members").Elements("member")
                .Where(x => x.Attribute("name").Value.StartsWith($"P:{typeName}"))
                .ToList();

            return results;
        }

        private HelpInfo CreateClassHelpInfo(XElement typeElement, IEnumerable<XElement> propertyElements)
        {
            var typeName = GetTypeName(typeElement);

            var subjectName = GetSubjectName(typeName);
            var actionName = GetActionName(typeName);

            return new HelpInfo(
                actionName,
                subjectName,
                typeElement.Element("summary")?.Value.Trim(),
                HelpInfoType.Executable,
                propertyElements
                    .Select(p => new HelpInfo(
                        $"{GetPropertyName(p).FirstCharToLowerCase()}",
                        $"{subjectName} {actionName}",
                        p.Element("summary")?.Value.Trim(),
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

        private string GetTypeFullName(XElement typeElement)
        {
            var name = typeElement.Attribute("name").Value;
            var start = name.IndexOf("T:") + "T:".Length;
            var end = name.Length;

            return name.Substring(start, end - start);
        }

        private string GetTypeName(XElement typeElement)
        {
            var name = typeElement.Attribute("name").Value;
            var start = name.LastIndexOf('.') + 1;
            var end = name.Length;

            return name.Substring(start, end - start);
        }

        private string GetPropertyName(XElement propertyElement)
        {
            var name = propertyElement.Attribute("name").Value;
            var start = name.LastIndexOf('.') + 1;
            var end = name.Length;

            return name.Substring(start, end - start);
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
