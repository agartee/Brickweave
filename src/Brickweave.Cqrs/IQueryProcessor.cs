using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IQueryProcessor
    {
        Task<object> ProcessAsync(IQuery query);
        Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query);
    }
}
