namespace Weapsy.Mediator.Queries
{
    /// <summary>
    /// IQueryDispatcher
    /// </summary>
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Gets the result.
        /// The query handler must implement IQueryHandler.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TResult</returns>
        TResult Dispatch<TQuery, TResult>(TQuery query) 
            where TQuery : IQuery;
    }
}
