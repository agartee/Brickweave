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

            if(!File.Exists(_filePath))
                throw new CategoryHelpFileNotFoundExeption();

            var data = TryDeserialize(File.ReadAllText(_filePath));

            var categories = data
                .Where(kvp => IsSubjectOrOneLevelDeeper(kvp.Key))
                .Select(kvp => new HelpInfo(
                    kvp.Key.IndexOf(' ') > -1
                        ? kvp.Key.Substring(kvp.Key.IndexOf(' ') + 1)
                        : kvp.Key,
                    kvp.Key.Split(' ').FirstOrDefault(),
                    kvp.Value,
                    HelpInfoType.Category))
                .ToList();

            if (adjacencyCriteria.Equals(HelpAdjacencyCriteria.Empty()))
                return _defaultHelpInfo.WithChildren(categories);

            if (categories.Any(h => h.Subject == adjacencyCriteria.Subject))
            {
                return categories
                    .First(h => h.Name == adjacencyCriteria.Subject)
                    .WithChildren(categories.Where(h => h.Name != adjacencyCriteria.Subject));
            }

            return null;
            
            Dictionary<string, string> TryDeserialize(string json)
            {
                try { return JsonConvert.DeserializeObject<Dictionary<string, string>>(json); }
                catch { throw new CategoryHelpFileInvalidExeption(); }
            }

            bool IsSubjectOrOneLevelDeeper(string key)
            {
                var keySplit = key.Split(' ');
                var adjacencySplit = adjacencyCriteria.Subject
                    .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                if (adjacencyCriteria.Subject == null && keySplit.Length == 1)
                    return true;

                return key.StartsWith(adjacencyCriteria.Subject)
                    && (keySplit.Length == adjacencySplit.Length || keySplit.Length == adjacencySplit.Length + 1);
            }
        }
    }
}