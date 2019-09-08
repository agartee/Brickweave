using Brickweave.Cqrs;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class ExportPeople : IQuery<string>
    {
        /// <summary>
        /// Exports all existing peoples' event streams.
        /// </summary>
        public ExportPeople() { }
    }
}
