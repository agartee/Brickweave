using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class CreateFooHandler : ICommandHandler<CreateFoo, string>
    {
        public Task<string> HandleAsync(CreateFoo command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult("success!");
        }
    }
}