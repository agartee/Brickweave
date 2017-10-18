using System;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class CreateFoo : ICommand<string>
    {
        public CreateFoo(int id, DateTime dateCreated, string bar = "bar")
        {
            Id = id;
            Bar = bar;
            DateCreated = dateCreated;
        }

        public int Id { get; }
        public string Bar { get; }
        public DateTime DateCreated { get; }
    }
}
