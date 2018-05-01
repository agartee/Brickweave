using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestSecuredCommandHandler : ISecuredCommandHandler<TestCommand>
    {
        private readonly Action _actionWhenCalled;
        private readonly Action _actionWhenCalledWithCancellationRequest;

        public TestSecuredCommandHandler(Action actionWhenCalled, Action actionWhenCalledWithCancellationRequest)
        {
            _actionWhenCalled = actionWhenCalled;
            _actionWhenCalledWithCancellationRequest = actionWhenCalledWithCancellationRequest;
        }

        public Task HandleAsync(TestCommand command, ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                _actionWhenCalledWithCancellationRequest?.Invoke();
            else
                _actionWhenCalled?.Invoke();

            return Task.CompletedTask;
        }
    }
}