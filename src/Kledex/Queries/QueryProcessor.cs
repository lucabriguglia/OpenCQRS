using Kledex.Caching;
using Kledex.Dependencies;
using Kledex.Exceptions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
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

        private static readonly ConcurrentDictionary<Type, object> _queryHandlerWrappers = new ConcurrentDictionary<Type, object>();

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
                var queryType = query.GetType();
                var handler = (BaseQueryHandlerWrapper<TResult>)_queryHandlerWrappers.GetOrAdd(queryType,
                    t => Activator.CreateInstance(typeof(QueryHandlerWrapper<,>).MakeGenericType(queryType, typeof(TResult))));
                return handler.HandleAsync(query, _handlerResolver);
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

        /// <inheritdoc />
        public TResult Process<TResult>(IQuery<TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            TResult GetResult(IQuery<TResult> query)
            {
                var queryType = query.GetType();
                var handler = (BaseQueryHandlerWrapper<TResult>)_queryHandlerWrappers.GetOrAdd(queryType,
                    t => Activator.CreateInstance(typeof(QueryHandlerWrapper<,>).MakeGenericType(queryType, typeof(TResult))));
                return handler.Handle(query, _handlerResolver);
            }

            if (query is ICacheableQuery<TResult> cacheableQuery)
            {
                if (string.IsNullOrEmpty(cacheableQuery.CacheKey))
                {
                    throw new QueryException("Cache key is required.");
                }

                return _cacheManager.GetOrSet(
                    cacheableQuery.CacheKey,
                    cacheableQuery.CacheTime ?? _options.CacheTime,
                    () => GetResult(query));
            }

            return GetResult(query);
        }
    }
}