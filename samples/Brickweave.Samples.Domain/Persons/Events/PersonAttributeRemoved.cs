using Brickweave.EventStore;
using Brickweave.Samples.Domain.Persons.Extensions;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonAttributeRemoved : IEvent
    {
        public const string ALL_VALUES = "*";

        public PersonAttributeRemoved(string name, object value)
        {
            Name = name;
            Value = value.CorrectType();
        }

        public string Name { get; }
        public object Value { get; }
    }
}
