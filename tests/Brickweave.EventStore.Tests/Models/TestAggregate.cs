using System;
using System.Collections.Generic;
using System.Linq;

namespace Brickweave.EventStore.Tests.Models
{
    public class TestAggregate : EventSourcedAggregateRoot
    {
        private readonly List<ChildEntity> _children = new List<ChildEntity>();

        TestAggregate()
        {
            Register<TestAggregateCreated>(Apply);
            Register<MiscEvent>(Apply);
            Register<ChildAdded>(Apply);
        }
        
        public TestAggregate(IEnumerable<IEvent> events) : this()
        {
            events.ToList().ForEach(ApplyEvent);
        }

        public TestAggregate(Guid id) : this()
        {
            RaiseEvent(new TestAggregateCreated(id));
        }

        public Guid Id { get; private set; }

        public IEnumerable<ChildEntity> Children => _children.ToArray();

        public void RaiseMiscEvent()
        {
            RaiseEvent(new MiscEvent());
        }

        public void RaiseUnregisteredEvent()
        {
            RaiseEvent(new UnregisteredEvent());
        }

        public void AddChild(int childId)
        {
            RaiseEvent(new ChildAdded(childId));
        }

        private void Apply(TestAggregateCreated @event)
        {
            Id = @event.TestId;
        }

        private void Apply(MiscEvent @event)
        {
            // nothing
        }

        private void Apply(ChildAdded @event)
        {
            _children.Add(new ChildEntity(@event.Id, EventQueue, EventRouter));
        }
    }
}
