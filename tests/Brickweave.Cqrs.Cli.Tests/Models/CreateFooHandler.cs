using System;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class CreateFooHandler : ICommandHandler<CreateFoo>
    {
        public Task HandleAsync(CreateFoo command)
        {
            throw new NotImplementedException();
        }
    }
}