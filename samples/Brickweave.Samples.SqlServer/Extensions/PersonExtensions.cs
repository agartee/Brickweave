using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.SqlServer.Entities;
using Brickweave.Samples.SqlServer.Serialization;

namespace Brickweave.Samples.SqlServer.Extensions
{
    public static class PersonExtensions
    {
        public static PersonSnapshot ToSnapshot(this Person person)
        {
            return new PersonSnapshot
            {
                Id = person.Id.Value,
                Json = SnapshotSerializer.SerializeObject(person.ToInfo())
            };
        }

        public static PersonInfo ToInfo(this PersonSnapshot data)
        {
            return SnapshotSerializer.DeserializeObject<PersonInfo>(data.Json);
        }
    }
}
