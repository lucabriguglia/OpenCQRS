using System;

namespace Kledex.Dependencies
{
    public interface IHandlerResolver
    {
        THandler ResolveHandler<THandler>();
        object ResolveHandler(Type handlerType);
    }
}
