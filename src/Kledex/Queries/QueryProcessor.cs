using System;
using System.Threading.Tasks;
using Kledex.Dependencies;

namespace Kledex.Queries
{
    /// <inheritdoc />
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IQueryHandlerResolver _queryHandlerResolver;
        private readonly IHandlerResolver _handlerResolver;

        public QueryProcessor(IQueryHandlerResolver queryHandlerResolver, IHandlerResolver handlerResolver)
        {
            _queryHandlerResolver = queryHandlerResolver;
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
        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _queryHandlerResolver.ResolveHandler(query, typeof(IQueryHandlerAsync2<,>));

            var handleMethod = handler.GetType().GetMethod("HandleAsync");

            return (Task<TResult>)handleMethod.Invoke(handler, new object[] { query });
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