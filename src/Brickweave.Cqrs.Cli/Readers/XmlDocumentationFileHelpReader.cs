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
    public class XmlDocumentationFileHelpReader : IExecutableHelpReader
    {
        private readonly string[] _filePaths;

        public XmlDocumentationFileHelpReader(params string[] filePaths)
        {
            _filePaths = filePaths;
        }
        
        public IEnumerable<HelpInfo> GetHelpInfo(HelpAdjacencyCriteria adjacencyCriteria)
        {
            return _filePaths
                .SelectMany(filePath => Read(filePath, adjacencyCriteria))
                .ToList();
        }

        private static IEnumerable<HelpInfo> Read(string filePath, HelpAdjacencyCriteria adjacencyCriteria)
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
                var subjectName = GetSubjectName(constructorElement);
                var actionName = GetActionName(constructorElement);

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

            string GetSubjectName(XElement constructorElement)
            {
                var result = constructorElement.Element("subject")?.Value;

                return result ?? string.Join(" ", SplitTypeName(GetTypeName(constructorElement)).Skip(1));
            }

            string GetActionName(XElement constructorElement)
            {
                var result = constructorElement.Element("action")?.Value;

                return result ?? SplitTypeName(GetTypeName(constructorElement)).First();
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
