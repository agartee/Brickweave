using System;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class GetPerson : IQuery<PersonInfo>
    {
        public GetPerson(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
