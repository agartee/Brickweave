using System;
using Brickweave.Cqrs;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class CreateFoo : ICommand
    {
        public CreateFoo(int id, DateTime created, string bar = "bar")
        {
            Id = id;
            Bar = bar;
            Created = created;
        }

        public int Id { get; }
        public string Bar { get; }
        public DateTime Created { get; }
    }
}
