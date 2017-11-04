using System.Threading.Tasks;

namespace Weapsy.Mediator.Queries
{
    public interface IQueryHandlerAsync<in TQuery, TResult> where TQuery : IQuery
    {
        Task<TResult> RetrieveAsync(TQuery query);
    }
}
