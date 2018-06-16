using System;
using OpenCqrs.Dependencies;

namespace OpenCqrs.Queries
{
    /// <inheritdoc />
    /// <summary>
    /// IQueryDispatcher
    /// </summary>
    /// <seealso cref="T:OpenCqrs.Queries.IQueryDispatcher" />
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
                throw new ApplicationException($"No handler of type OpenCqrs.Queries.IQueryHandler<TQuery, TResult> found for query '{query.GetType().FullName}'");

            return handler.Retrieve(query);
        }
    }
}