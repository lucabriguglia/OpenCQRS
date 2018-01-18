using System.Collections.Generic;

namespace Weapsy.Cqrs.Dependencies
{
    public interface IResolver
    {
        T Resolve<T>();
        IEnumerable<T> ResolveAll<T>();
    }
}
