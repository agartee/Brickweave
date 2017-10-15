using System;
using System.Threading.Tasks;
using Brickweave.Cqrs;

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