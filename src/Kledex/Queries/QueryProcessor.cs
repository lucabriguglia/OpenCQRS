using Kledex.Caching;
using Kledex.Dependencies;
using Kledex.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Options = Kledex.Configuration.Options;

namespace Kledex.Queries
{
    /// <inheritdoc />
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IHandlerResolver _handlerResolver;
        private readonly ICacheManager _cacheManager;
        private readonly Options _options;

        public QueryProcessor(IHandlerResolver handlerResolver, 
            ICacheManager cacheManager, 
            IOptions<Options> options)
        {
            _handlerResolver = handlerResolver;
            _cacheManager = cacheManager;
            _options = options.Value;
        }

        /// <inheritdoc />
        public Task<TResult> ProcessAsync<TResult>(IQuery<TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            Task<TResult> GetResultAsync(IQuery<TResult> query)
            {
                var handler = _handlerResolver.ResolveQueryHandler(query, typeof(IQueryHandlerAsync<,>));
                var handleMethod = handler.GetType().GetMethod("HandleAsync", new[] { query.GetType() });
                return (Task<TResult>)handleMethod.Invoke(handler, new object[] { query });
            }

            if (query is ICacheableQuery<TResult> cacheableQuery)
            {
                if (string.IsNullOrEmpty(cacheableQuery.CacheKey))
                {
                    throw new QueryException("Cache key is required.");
                }

                return _cacheManager.GetOrSetAsync(
                    cacheableQuery.CacheKey, 
                    cacheableQuery.CacheTime ?? _options.CacheTime, 
                    () => GetResultAsync(query));
            }

            return GetResultAsync(query);
        }
    }
}