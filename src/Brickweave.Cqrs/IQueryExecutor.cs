using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IQueryExecutor
    {
        Task<object> ExecuteAsync(IQuery query);
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}
