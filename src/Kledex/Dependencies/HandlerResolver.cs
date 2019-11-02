using System;
using System.Linq;
using Kledex.Exceptions;

namespace Kledex.Dependencies
{
    public class HandlerResolver : IHandlerResolver
    {
        private readonly IResolver _resolver;

        public HandlerResolver(IResolver resolver)
        {
            _resolver = resolver;
        }

        public THandler ResolveHandler<THandler>()
        {
            var handler = _resolver.Resolve<THandler>();

            if (handler == null)
                throw new HandlerNotFoundException(typeof(THandler));

            return handler;
        }

        public object ResolveHandler(Type handlerType)
        {
            var handler = _resolver.Resolve(handlerType);

            if (handler == null)
                throw new HandlerNotFoundException(handlerType);

            return handler;
        }

        public object ResolveCommandHandler(object command, Type type)
        {
            var commandType = command.GetType();
            var handlerType = type.MakeGenericType(commandType);
            return ResolveHandler(handlerType);
        }

        public object ResolveQueryHandler(object query, Type type)
        {
            var queryType = query.GetType();
            var queryInterface = queryType.GetInterfaces()[0];
            var resultType = queryInterface.GetGenericArguments().FirstOrDefault();
            var handlerType = type.MakeGenericType(queryType, resultType);
            return ResolveHandler(handlerType);
        }
    }
}