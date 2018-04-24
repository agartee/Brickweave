using System;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Commands;

namespace Brickweave.Samples.Domain.Projections
{
    public class CreatePersonProjectionHandler : IProjectionHandler<CreatePerson>
    {
        public Task HandleAsync(CreatePerson command)
        {
            Console.WriteLine("Fart: {0}", command.Name);

            return Task.CompletedTask;
        }
    }
}
