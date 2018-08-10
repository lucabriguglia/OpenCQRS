using System;

namespace OpenCqrs.Exceptions
{
    public class ConcurrencyException : ApplicationException
    {
        public ConcurrencyException(int expectedVersion, int actualVersion)
            : base(BuildErrorMesage(expectedVersion, actualVersion))
        {
        }

        private static string BuildErrorMesage(int expectedVersion, int actualVersion)
        {
            return $"Expected version: '{expectedVersion}' | Actual version: '{actualVersion}'";
        }
    }
}