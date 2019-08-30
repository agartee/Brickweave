using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class ImportPerson : ICommand<PersonInfo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public ImportPerson(string json)
        {
            Json = json;
        }

        public string Json { get; }
    }
}
