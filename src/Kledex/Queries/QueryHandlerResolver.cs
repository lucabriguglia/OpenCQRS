using System;
using System.Linq;
using Kledex.Dependencies;

namespace Kledex.Queries
{
    public class QueryHandlerResolver : IQueryHandlerResolver
    {
        private readonly IHandlerResolver _handlerResolver;

        public QueryHandlerResolver(IHandlerResolver handlerResolver)
        {
            _handlerResolver = handlerResolver;
        }

        public object ResolveHandler(object query, Type type)
        {
            var queryType = query.GetType();
            var queryInterface = queryType.GetInterfaces()[0];
            var resultType = queryInterface.GetGenericArguments().FirstOrDefault();
            var handlerType = type.MakeGenericType(queryType, resultType);
            return _handlerResolver.ResolveHandler(handlerType);
        }
    }
}