using System.Threading.Tasks;
using Brickweave.Cqrs;

namespace Brickweave.Samples.Domain.Sandbox.Commands
{
    public class CreateFooHandler : ICommandHandler<CreateFoo>
    {
        public Task HandleAsync(CreateFoo command)
        {
            return Task.CompletedTask;
        }
    }
}
