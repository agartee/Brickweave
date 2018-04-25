using System;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestProjectionHandler : IProjectionHandler<TestCommand>
    {
        private readonly Action _actionWhenCalled;

        public TestProjectionHandler(Action actionWhenCalled)
        {
            _actionWhenCalled = actionWhenCalled;
        }

        public Task HandleAsync(TestCommand command)
        {
            _actionWhenCalled?.Invoke();

            return Task.CompletedTask;
        }
    }
}