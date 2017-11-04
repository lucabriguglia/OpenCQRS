using System.Collections.Generic;

namespace Weapsy.Mediator.Dependencies
{
    public interface IResolver
    {
        T Resolve<T>();
        IEnumerable<T> ResolveAll<T>();
    }
}
