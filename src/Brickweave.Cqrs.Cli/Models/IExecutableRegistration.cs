using System;

namespace Brickweave.Cqrs.Cli.Models
{
    public interface IExecutableRegistration
    {
        Type Type { get; }
        string ActionName { get; }
        string SubjectName { get; }
    }
}
