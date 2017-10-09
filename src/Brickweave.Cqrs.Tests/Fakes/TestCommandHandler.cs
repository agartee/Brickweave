using System;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Fakes
{
    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        private readonly Action _actionWhenCalled;

        public TestCommandHandler(Action actionWhenCalled)
        {
            _actionWhenCalled = actionWhenCalled;
        }

        public async Task HandleAsync(TestCommand command)
        {
            _actionWhenCalled?.Invoke();
        }
    }
}