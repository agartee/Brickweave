using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicCqrs.Domain.People.Models;

namespace BasicCqrs.Domain.People.Services
{
    /// <summary>
    /// For this demo, real database reads/writes are not necessary.
    /// </summary>
    public class DummyPersonRepository : IPersonRepository
    {
        private readonly ConcurrentDictionary<PersonId, Person> items
            = new ConcurrentDictionary<PersonId, Person>();

        public DummyPersonRepository()
        {
            // Add a default person for demo purposes
            var initialPerson = new Person(PersonId.NewId()) { FirstName = "Adam", LastName = "Gartee" };
            items.AddOrUpdate(initialPerson.Id, initialPerson, (id, p) => initialPerson);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously (normally, DB operations should be handled asynchronously)

        public async Task<Person> GetPersonAsync(PersonId id)
        {
            if (!items.ContainsKey(id))
                throw new InvalidOperationException($"Person with ID, {id} does not exist.");

            return items[id];
        }

        public async Task<IEnumerable<Person>> ListPeopleAsync(PersonSearchCriteria criteria)
        {
            return items
                .Where(p => p.Value.FullName.Contains(criteria.NameLike))
                .Select(kvp => kvp.Value)
                .ToArray();
        }

        public async Task SavePersonAsync(Person person)
        {
            items.AddOrUpdate(person.Id, person, (id, p) => person);
        }

        public async Task DeletePerson(PersonId id)
        {
            if (!items.ContainsKey(id))
                throw new InvalidOperationException($"Person with ID, {id} does not exist.");

            items.TryRemove(id, out var person);
        }

#pragma warning restore CS1998
    }
}
