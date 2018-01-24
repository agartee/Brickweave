using System.Collections.Generic;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Sandbox.Models;

namespace Brickweave.Samples.Domain.Sandbox.Commands
{
    public class CreateFoo : ICommand
    {
        public CreateFoo(IList<FooId> ids)
        {
            Ids = ids;
        }

        public IList<FooId> Ids { get; }
    }
}
