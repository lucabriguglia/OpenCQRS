using System;
using OpenCqrs.Dependencies;
using OpenCqrs.Exceptions;

namespace OpenCqrs.Commands
{
    public abstract class BaseCommandSender
    {
        private readonly IResolver _resolver;

        protected BaseCommandSender(IResolver resolver)
        {
            _resolver = resolver;
        }

        protected THandler GetHandler<THandler>(object command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var handler = _resolver.Resolve<THandler>();
                
            if (handler == null)
                throw new HandlerNotFoundException(typeof(THandler));

            return handler;
        }
    }
}
