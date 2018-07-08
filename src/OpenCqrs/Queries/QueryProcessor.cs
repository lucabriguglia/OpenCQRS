using System;
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
        public TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _handlerResolver.ResolveHandler<IQueryHandler<TQuery, TResult>>();
            
            return handler.Retrieve(query);
        }
    }
}