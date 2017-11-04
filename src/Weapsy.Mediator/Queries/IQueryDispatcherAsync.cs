using System.Threading.Tasks;

namespace Weapsy.Mediator.Queries
{
    /// <summary>
    /// IQueryDispatcherAsync
    /// </summary>
    public interface IQueryDispatcherAsync
    {
        /// <summary>
        /// Asynchronously gets the result.
        /// The query handler must implement IQueryHandlerAsync.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TResult</returns>
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) 
            where TQuery : IQuery;
    }
}
