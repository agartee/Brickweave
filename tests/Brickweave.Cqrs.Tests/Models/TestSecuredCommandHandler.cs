using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestSecuredCommandHandler : ISecuredCommandHandler<TestCommand>
    {
        private readonly Action _actionWhenCalled;

        public TestSecuredCommandHandler(Action actionWhenCalled)
        {
            _actionWhenCalled = actionWhenCalled;
        }

        public Task HandleAsync(TestCommand command, ClaimsPrincipal user)
        {
            _actionWhenCalled?.Invoke();

            return Task.CompletedTask;
        }
    }
}