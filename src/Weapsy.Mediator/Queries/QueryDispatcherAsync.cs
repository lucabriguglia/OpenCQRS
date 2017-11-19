using System;
using System.Threading.Tasks;
using Weapsy.Mediator.Dependencies;

namespace Weapsy.Mediator.Queries
{
    /// <inheritdoc />
    /// <summary>
    /// QueryDispatcherAsync
    /// </summary>
    /// <seealso cref="T:Weapsy.Mediator.Queries.IQueryDispatcherAsync" />
    public class QueryDispatcherAsync : IQueryDispatcherAsync
    {
        private readonly IResolver _resolver;

        public QueryDispatcherAsync(IResolver resolver)
        {
            _resolver = resolver;
        }

        public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
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