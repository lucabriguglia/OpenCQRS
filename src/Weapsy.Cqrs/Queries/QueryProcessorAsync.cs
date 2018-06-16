using System;
using System.Threading.Tasks;
using OpenCqrs.Dependencies;

namespace OpenCqrs.Queries
{
    /// <inheritdoc />
    /// <summary>
    /// QueryDispatcherAsync
    /// </summary>
    /// <seealso cref="T:OpenCqrs.Queries.IQueryDispatcherAsync" />
    public class QueryProcessorAsync : IQueryProcessorAsync
    {
        private readonly IResolver _resolver;

        public QueryProcessorAsync(IResolver resolver)
        {
            _resolver = resolver;
        }

        /// <inheritdoc />
        public async Task<TResult> ProcessAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _resolver.Resolve<IQueryHandlerAsync<TQuery, TResult>>();

            if (handler == null)
                throw new ApplicationException($"No handler of type OpenCqrs.Queries.IQueryHandlerAsync<TQuery, TResult>> found for query '{query.GetType().FullName}'");

            return await handler.RetrieveAsync(query);
        }
    }
}