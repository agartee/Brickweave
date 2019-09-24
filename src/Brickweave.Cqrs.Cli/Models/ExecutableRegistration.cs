using System;
using System.Linq;

namespace Brickweave.Cqrs.Cli.Models
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
}
