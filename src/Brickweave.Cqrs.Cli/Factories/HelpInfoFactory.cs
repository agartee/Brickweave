using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Readers;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class HelpInfoFactory : IHelpInfoFactory
    {
        private readonly ICategoryHelpReader _categoryHelpReader;
        private readonly IEnumerable<IExecutableHelpReader> _executableHelpReaders;
        
        public HelpInfoFactory(ICategoryHelpReader categoryHelpReader,
            IEnumerable<IExecutableHelpReader> executableHelpReaders)
        {
            _categoryHelpReader = categoryHelpReader;
            _executableHelpReaders = executableHelpReaders;
        }
        
        public HelpInfo Create(string[] args)
        {
            var argsWithoutParams = args
                .Take(GetFirstParamIndex(args))
                .ToArray();

            var categoryBySubjectCriteria = CreateSubjectCriteria(argsWithoutParams);
            var categoryBySubject = _categoryHelpReader.GetHelpInfo(
                categoryBySubjectCriteria);

            if (categoryBySubject != null)
                return GetCategoryHelpInfoWithChildren(categoryBySubjectCriteria, categoryBySubject);
            
            var executablesBySubject = _executableHelpReaders
                .SelectMany(r => r.GetHelpInfo(categoryBySubjectCriteria))
                .OrderBy(h => h.Name)
                .ToArray();

            if (executablesBySubject.Any())
            {
                return new HelpInfo(
                    categoryBySubjectCriteria.Subject, 
                    categoryBySubjectCriteria.Subject,
                    string.Empty, 
                    HelpInfoType.Category, 
                    executablesBySubject);
            }

            var executableBySubjectAndAction = _executableHelpReaders
                .SelectMany(r => r.GetHelpInfo(CreateSubjectAndActionCriteria(argsWithoutParams)))
                .OrderBy(h => h.Name)
                .FirstOrDefault();

            return executableBySubjectAndAction;
        }

        private int GetFirstParamIndex(string[] args)
        {
            return args
                .Select((arg, index) => new { Value = arg, Index = index })
                .FirstOrDefault(arg => arg.Value.StartsWith("-"))?.Index
                ?? args.Length;
        }

        private HelpAdjacencyCriteria CreateSubjectCriteria(string[] argsWithoutParams)
        {
            var subject = string.Join(" ", argsWithoutParams);

            return !string.IsNullOrWhiteSpace(subject)
                ? new HelpAdjacencyCriteria(subject)
                : HelpAdjacencyCriteria.Empty();
        }

        private HelpAdjacencyCriteria CreateSubjectAndActionCriteria(string[] argsWithoutParams)
        {
            var subject = string.Join(" ", argsWithoutParams.Take(argsWithoutParams.Length - 1));
            var action = argsWithoutParams.Last();

            return new HelpAdjacencyCriteria(subject, action);
        }

        private HelpInfo GetCategoryHelpInfoWithChildren(HelpAdjacencyCriteria categoryBySubjectCriteria,
            HelpInfo categoryBySubject)
        {
            var childExecutables = _executableHelpReaders
                .SelectMany(r => r.GetHelpInfo(categoryBySubjectCriteria))
                .OrderBy(h => h.Name)
                .ToList();

            return categoryBySubject.WithChildren(
                categoryBySubject.Children
                    .Union(childExecutables)
                    .ToList());
        }
    }
}
