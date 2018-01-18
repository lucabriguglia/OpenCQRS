using System;
using System.Threading.Tasks;
using Weapsy.Cqrs.Dependencies;

namespace Weapsy.Cqrs.Queries
{
    /// <inheritdoc />
    /// <summary>
    /// QueryDispatcherAsync
    /// </summary>
    /// <seealso cref="T:Weapsy.Cqrs.Queries.IQueryDispatcherAsync" />
    public class QueryProcessorAsync : IQueryProcessorAsync
    {
        private readonly IResolver _resolver;

        public QueryProcessorAsync(IResolver resolver)
        {
            _resolver = resolver;
        }

        public async Task<TResult> ProcessAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var queryHandler = _resolver.Resolve<IQueryHandlerAsync<TQuery, TResult>>();

            if (queryHandler == null)
                throw new ApplicationException($"No handler of type IQueryHandlerAsync<TQuery, TResult>> found for query '{query.GetType().FullName}'");

            return await queryHandler.RetrieveAsync(query);
        }
    }
}