using System;
using OpenCqrs.Dependencies;
using OpenCqrs.Exceptions;

namespace OpenCqrs.Queries
{
    /// <inheritdoc />
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IResolver _resolver;

        public QueryProcessor(IResolver resolver)
        {
            _resolver = resolver;
        }

        /// <inheritdoc />
        public TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _resolver.Resolve<IQueryHandler<TQuery, TResult>>();

            if (handler == null)
                throw new HandlerNotFoundException(typeof(IQueryHandler<TQuery, TResult>));

            return handler.Retrieve(query);
        }
    }
}