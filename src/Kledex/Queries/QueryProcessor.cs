using Kledex.Dependencies;
using System;
using System.Threading.Tasks;

namespace Kledex.Queries
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
        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _handlerResolver.ResolveQueryHandler(query, typeof(IQueryHandlerAsync<,>));

            var handleMethod = handler.GetType().GetMethod("HandleAsync");

            return (Task<TResult>)handleMethod.Invoke(handler, new object[] { query });
        }

        /// <inheritdoc />
        public TResult Process<TResult>(IQuery<TResult> query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _handlerResolver.ResolveQueryHandler(query, typeof(IQueryHandler<,>));

            var handleMethod = handler.GetType().GetMethod("Handle");

            return (TResult)handleMethod.Invoke(handler, new object[] { query });
        }
    }
}