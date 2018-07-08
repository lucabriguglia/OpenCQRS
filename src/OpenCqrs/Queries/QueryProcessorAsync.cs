using System;
using System.Threading.Tasks;
using OpenCqrs.Dependencies;

namespace OpenCqrs.Queries
{
    /// <inheritdoc />
    public class QueryProcessorAsync : IQueryProcessorAsync
    {
        private readonly IHandlerResolver _handlerResolver;

        public QueryProcessorAsync(IHandlerResolver handlerResolver)
        {
            _handlerResolver = handlerResolver;
        }

        /// <inheritdoc />
        public Task<TResult> ProcessAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _handlerResolver.ResolveHandler<IQueryHandlerAsync<TQuery, TResult>>();

            return handler.RetrieveAsync(query);
        }
    }
}