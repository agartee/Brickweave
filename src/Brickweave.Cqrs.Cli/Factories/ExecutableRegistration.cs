using System;
using System.Linq;

namespace Brickweave.Cqrs.Cli.Factories
{
    public class ExecutableRegistration<T> : IExecutableRegistration where T : class, IExecutable 
    {
        public ExecutableRegistration(string actionName, params string[] subjectNameParts)
        {
            Type = typeof(T);
            ActionName = actionName;
            SubjectName = string.Join(" ", subjectNameParts.Select(p => p.Trim().ToLower()));
        }

        public Type Type { get; }
        public string ActionName { get; }
        public string SubjectName { get; }
    }

    public interface IExecutableRegistration
    {
        Type Type { get; }
        string ActionName { get; }
        string SubjectName { get; }
    }
}
