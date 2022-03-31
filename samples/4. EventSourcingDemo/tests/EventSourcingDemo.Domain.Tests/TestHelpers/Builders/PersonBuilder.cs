using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.Tests.TestHelpers.Builders
{
    public class PersonBuilder
    {
        private PersonId _id = PersonId.NewId();
        private Name _name;

        public PersonBuilder WithId(PersonId id)
        {
            _id = id;
            return this;
        }

        public PersonBuilder WithName(Name name)
        {
            _name = name;
            return this;
        }

        public Person Build()
        {
            return new Person(
                _id, 
                _name ?? new Name(_id.ToString()));
        }
    }
}
