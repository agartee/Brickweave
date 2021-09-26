using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Brickweave.Cqrs.Cli.Exceptions;
using Brickweave.Cqrs.Cli.Models;
using LiteGuard;
using Newtonsoft.Json;

namespace Brickweave.Cqrs.Cli.Readers
{
    public class JsonFileCategoryHelpReader : ICategoryHelpReader
    {
        private readonly string _filePath;
        private readonly HelpInfo _defaultHelpInfo;

        public JsonFileCategoryHelpReader(string filePath, HelpInfo defaultHelpInfo = null)
        {
            _filePath = filePath;
            _defaultHelpInfo = defaultHelpInfo 
                ?? new HelpInfo("root", string.Empty, string.Empty, HelpInfoType.Category);
        }

        public HelpInfo GetHelpInfo(HelpAdjacencyCriteria adjacencyCriteria)
        {
            Guard.AgainstNullArgument(nameof(adjacencyCriteria), adjacencyCriteria);

            if (!File.Exists(_filePath))
                throw new CategoryHelpFileNotFoundExeption();

            var data = TryDeserialize(File.ReadAllText(_filePath));

            var categories = data
                .Where(kvp => IsSubjectOrOneLevelDeeper(adjacencyCriteria, kvp.Key))
                .Select(kvp => new
                {
                    Name = GetName(kvp),
                    FullName = kvp.Key,
                    Subject = GetSubject(kvp),
                    Description = kvp.Value,
                    Type = HelpInfoType.Category
                }).ToList();

            if (adjacencyCriteria.Equals(HelpAdjacencyCriteria.Empty()))
            {
                var children = categories
                    .Select(o => new HelpInfo(o.Name, o.Subject, o.Description, o.Type));

                return _defaultHelpInfo.WithChildren(children);
            }
            else
            {
                var children = categories
                    .Where(h => h.FullName != adjacencyCriteria.Subject)
                    .Select(o => new HelpInfo(o.Name, o.Subject, o.Description, o.Type))
                    .ToList();

                return categories
                    .Where(h => h.FullName == adjacencyCriteria.Subject)
                    .Select(o => new HelpInfo(o.Name, o.Subject, o.Description, o.Type))
                    .FirstOrDefault()?.WithChildren(children);
            }
        }

        private Dictionary<string, string> TryDeserialize(string json)
        {
            try { return JsonConvert.DeserializeObject<Dictionary<string, string>>(json); }
            catch { throw new CategoryHelpFileInvalidExeption(); }
        }

        private bool IsSubjectOrOneLevelDeeper(HelpAdjacencyCriteria adjacencyCriteria, string key)
        {
            var keySplit = key.Split(' ');
            var adjacencySplit = adjacencyCriteria.Subject
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (adjacencyCriteria.Subject == null && keySplit.Length == 1)
                return true;

            return key.StartsWith(adjacencyCriteria.Subject)
                && (keySplit.Length == adjacencySplit.Length || keySplit.Length == adjacencySplit.Length + 1);
        }

        private string GetName(KeyValuePair<string, string> kvp)
        {
            var keySplit = kvp.Key.Split(' ');

            if (keySplit.Count() == 1)
                return kvp.Key;

            return kvp.Key.Substring(kvp.Key.LastIndexOf(' ') + 1);
        }

        private string GetSubject(KeyValuePair<string, string> kvp)
        {
            var keySplit = kvp.Key.Split(' ');
            int keyPartCount = keySplit.Count();

            if (keyPartCount == 1)
                return kvp.Key;

            return keySplit[keyPartCount - 2];
        }
    }
}
