using System;
using Weapsy.Cqrs.Dependencies;

namespace Weapsy.Cqrs.Queries
{
    /// <inheritdoc />
    /// <summary>
    /// IQueryDispatcher
    /// </summary>
    /// <seealso cref="T:Weapsy.Cqrs.Queries.IQueryDispatcher" />
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IResolver _resolver;

        public QueryProcessor(IResolver resolver)
        {
            _resolver = resolver;
        }

        public TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var queryHandler = _resolver.Resolve<IQueryHandler<TQuery, TResult>>();

            if (queryHandler == null)
                throw new ApplicationException($"No handler of type IQueryHandler<TQuery, TResult> found for query '{query.GetType().FullName}'");

            return queryHandler.Retrieve(query);
        }
    }
}