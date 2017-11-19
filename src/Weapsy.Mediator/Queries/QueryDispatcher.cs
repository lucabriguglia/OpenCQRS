using System;
using Weapsy.Mediator.Dependencies;

namespace Weapsy.Mediator.Queries
{
    /// <inheritdoc />
    /// <summary>
    /// IQueryDispatcher
    /// </summary>
    /// <seealso cref="T:Weapsy.Mediator.Queries.IQueryDispatcher" />
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IResolver _resolver;

        public QueryDispatcher(IResolver resolver)
        {
            _resolver = resolver;
        }

        public TResult Dispatch<TQuery, TResult>(TQuery query) where TQuery : IQuery
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