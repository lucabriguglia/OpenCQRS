using System.Threading.Tasks;

namespace OpenCqrs.Queries
{
    /// <summary>
    /// IQueryProcessorAsync
    /// </summary>
    public interface IQueryProcessorAsync
    {
        /// <summary>
        /// Asynchronously gets the result.
        /// The query handler must implement OpenCqrs.Queries.IQueryHandlerAsync&lt;TQuery, TResult&gt;.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TResult</returns>
        Task<TResult> ProcessAsync<TQuery, TResult>(TQuery query) 
            where TQuery : IQuery;
    }
}
