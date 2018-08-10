using System;

namespace OpenCqrs.Exceptions
{
    public class ConcurrencyException : ApplicationException
    {
        public ConcurrencyException(long expectedVersion, long actualVersion)
            : base(BuildErrorMesage(expectedVersion, actualVersion))
        {
        }

        private static string BuildErrorMesage(long expectedVersion, long actualVersion)
        {
            return $"Expected version: '{expectedVersion}' | Actual version: '{actualVersion}'";
        }
    }
}