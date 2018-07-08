using System;
using OpenCqrs.Dependencies;
using OpenCqrs.Exceptions;

namespace OpenCqrs.Queries
{
    public abstract class BaseQueryProcessor
    {
        private readonly IResolver _resolver;

        protected BaseQueryProcessor(IResolver resolver)
        {
            _resolver = resolver;
        }

        protected THandler GetHandler<THandler>(object query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var handler = _resolver.Resolve<THandler>();

            if (handler == null)
                throw new HandlerNotFoundException(typeof(THandler));

            return handler;
        }
    }
}
