using System;
using System.Threading.Tasks;
using OpenCqrs.Dependencies;
using OpenCqrs.Exceptions;

namespace OpenCqrs.Queries
{
    /// <inheritdoc />
    public class QueryProcessorAsync : IQueryProcessorAsync
    {
        private readonly IResolver _resolver;

        public QueryProcessorAsync(IResolver resolver)
        {
            _resolver = resolver;
        }

        /// <inheritdoc />
        public Task<TResult> ProcessAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _resolver.Resolve<IQueryHandlerAsync<TQuery, TResult>>();

            if (handler == null)
                throw new HandlerNotFoundException(typeof(IQueryHandlerAsync<TQuery, TResult>));

            return handler.RetrieveAsync(query);
        }
    }
}