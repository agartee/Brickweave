namespace Brickweave.Cqrs
{
    public interface IQuery
    {
    }

    public interface IQuery<TResult> : IQuery
    {
    }
}
