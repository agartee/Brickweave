namespace Brickweave.Cqrs
{
    public interface IQuery : IExecutable
    {
    }

    public interface IQuery<TResult> : IQuery
    {
    }
}
