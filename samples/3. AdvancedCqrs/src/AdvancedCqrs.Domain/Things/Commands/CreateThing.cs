using System;
using AdvancedCqrs.Domain.Things.Models;
using Brickweave.Cqrs;
using Brickweave.Cqrs.Attributes;

namespace AdvancedCqrs.Domain.Things.Commands
{
    [LongRunning]
    public class CreateThing : ICommand<Thing>
    {
        public CreateThing(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }
    }
}
