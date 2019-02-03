using System;
using System.Threading.Tasks;
using OpenCqrs.Abstractions.Queries;
using OpenCqrs.Dependencies;

namespace OpenCqrs.Queries
{
    /// <inheritdoc />
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IHandlerResolver _handlerResolver;

        public QueryProcessor(IHandlerResolver handlerResolver)
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

        /// <inheritdoc />
        public TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _handlerResolver.ResolveHandler<IQueryHandler<TQuery, TResult>>();
            
            return handler.Retrieve(query);
        }
    }
}