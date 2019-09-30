using System;
using System.Collections.Generic;

namespace Kledex.Dependencies
{
    public interface IResolver
    {
        T Resolve<T>();
        IEnumerable<T> ResolveAll<T>();
        object Resolve(Type type);
    }
}
