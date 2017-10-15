namespace Brickweave.Cqrs
{
    public interface ICommand : IExecutable
    {
    }

    public interface ICommand<TResult> : ICommand
    {
    }
}
