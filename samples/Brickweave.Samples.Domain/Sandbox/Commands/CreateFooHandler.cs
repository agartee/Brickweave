using System.Threading;
using System.Threading.Tasks;
using Brickweave.Cqrs;

namespace Brickweave.Samples.Domain.Sandbox.Commands
{
    public class CreateFooHandler : ICommandHandler<CreateFoo>
    {
        public Task HandleAsync(CreateFoo command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}
