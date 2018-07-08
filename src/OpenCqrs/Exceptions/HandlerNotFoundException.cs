using System;

namespace OpenCqrs.Exceptions
{
    public class HandlerNotFoundException : ApplicationException
    {
        public HandlerNotFoundException(Type handlerType)
            : base(BuildErrorMesage(handlerType))
        {
        }

        private static string BuildErrorMesage(Type handlerType)
        {
            return $"No handler found that implements '{handlerType.FullName}'";
        }
    }
}