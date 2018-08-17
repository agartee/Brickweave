using System.Linq;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Readers;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class HelpInfoFactory : IHelpInfoFactory
    {
        private readonly ICategoryHelpReader _categoryHelpReader;
        private readonly IExecutableHelpReader _executableHelpReader;
        
        public HelpInfoFactory(ICategoryHelpReader categoryHelpReader, 
            IExecutableHelpReader executableHelpReader)
        {
            _categoryHelpReader = categoryHelpReader;
            _executableHelpReader = executableHelpReader;
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
            
            var executablesBySubject = _executableHelpReader
                .GetHelpInfo(categoryBySubjectCriteria)
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

            var executableBySubjectAndAction = _executableHelpReader
                .GetHelpInfo(CreateSubjectAndActionCriteria(argsWithoutParams))
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
            var childExecutables = _executableHelpReader
                .GetHelpInfo(categoryBySubjectCriteria)
                .ToList();

            return categoryBySubject.WithChildren(
                categoryBySubject.Children.Union(childExecutables));
        }
    }
}
