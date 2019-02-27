using Brickweave.EventStore;
using Brickweave.Samples.Domain.Persons.Extensions;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonAttributeAdded : IEvent
    {
        public PersonAttributeAdded(string name, object value)
        {
            Name = name;
            Value = value.CorrectType();
        }

        public string Name { get; }
        public object Value { get; }
    }
}
