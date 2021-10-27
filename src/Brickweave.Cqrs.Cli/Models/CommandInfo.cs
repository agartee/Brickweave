using System;
using System.Security.Claims;

namespace Brickweave.Cqrs.Cli.Models
{
    public class CommandInfo
    {
        public CommandInfo(Guid id, ICommand value, ClaimsPrincipal principal)
        {
            Id = id;
            Value = value;
            Principal = principal;
        }

        public Guid Id { get; }
        public ICommand Value { get; }
        public ClaimsPrincipal Principal { get; }
    }
}
