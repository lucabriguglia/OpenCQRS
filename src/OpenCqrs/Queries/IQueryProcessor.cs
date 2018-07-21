namespace OpenCqrs.Queries
{
    /// <summary>
    /// IQueryProcessor
    /// </summary>
    public interface IQueryProcessor
    {
        /// <summary>
        /// Gets the result.
        /// The query handler must implement OpenCqrs.Queries.IQueryHandler&lt;TQuery, TResult&gt;.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TResult</returns>
        TResult Process<TQuery, TResult>(TQuery query) 
            where TQuery : IQuery;
    }
}
