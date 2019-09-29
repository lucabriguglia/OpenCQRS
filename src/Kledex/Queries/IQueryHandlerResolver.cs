using System;

namespace Kledex.Queries
{
    public interface IQueryHandlerResolver
    {
        object ResolveHandler(object query, Type type);
    }
}
