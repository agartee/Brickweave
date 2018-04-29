using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class GetFooHandler : IQueryHandler<GetFoo, string>
    {
        public Task<string> HandleAsync(GetFoo query, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult("success!");
        }
    }
}