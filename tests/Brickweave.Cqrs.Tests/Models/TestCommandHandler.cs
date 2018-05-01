using System;
using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        private readonly Action _actionWhenCalled;
        private readonly Action _actionWhenCancellationRequested;

        public TestCommandHandler(Action actionWhenCalled, Action actionWhenCancellationRequested)
        {
            _actionWhenCalled = actionWhenCalled;
            _actionWhenCancellationRequested = actionWhenCancellationRequested;
        }

        public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                _actionWhenCancellationRequested?.Invoke();
            else
                _actionWhenCalled?.Invoke();

            return Task.CompletedTask;
        }
    }
}