namespace Brickweave.Cqrs
{
    public interface ICommand
    {
    }

    public interface ICommand<TResult> : ICommand
    {
    }
}
