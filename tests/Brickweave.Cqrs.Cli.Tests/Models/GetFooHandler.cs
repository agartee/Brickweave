using System.Threading.Tasks;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class GetFooHandler : IQueryHandler<GetFoo, string>
    {
        public Task<string> HandleAsync(GetFoo query)
        {
            return Task.FromResult("success!");
        }
    }
}