using System;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Cli
{
    [Obsolete("Due to potential for long-running commands, usage should be replaced with the standard IDispatcher with additional Task logic.")]
    public interface ICliDispatcher
    {
        Task<object> DispatchAsync(string commandText);
    }
}