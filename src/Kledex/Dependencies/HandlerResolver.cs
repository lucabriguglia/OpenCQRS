using System;
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

        public object ResolveHandler(object request, Type type)
        {
            var requestType = request.GetType();
            var handlerType = type.MakeGenericType(requestType);

            var handler = _resolver.Resolve(handlerType);

            if (handler == null)
                throw new HandlerNotFoundException(handlerType);

            return handler;
        }
    }
}