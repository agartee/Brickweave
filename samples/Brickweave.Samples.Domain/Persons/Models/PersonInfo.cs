using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Brickweave.Samples.Domain.Persons.Extensions;
using Brickweave.Samples.Domain.Phones.Models;

namespace Brickweave.Samples.Domain.Persons.Models
{
    public class PersonInfo
    {
        public PersonInfo(PersonId id, Name name, DateTime? birthDate, IEnumerable<PhoneInfo> phones,
            IReadOnlyDictionary<string, IEnumerable<object>> attributes)
        {
            Id = id;
            Name = name;
            BirthDate = birthDate;
            Phones = phones;
            Attributes = attributes;
        }

        public PersonId Id { get; }
        public Name Name { get; }
        public DateTime? BirthDate { get; }
        public IEnumerable<PhoneInfo> Phones { get; }
        public IReadOnlyDictionary<string, IEnumerable<object>> Attributes { get; }
    }
}
