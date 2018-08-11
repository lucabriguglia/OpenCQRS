using System;

namespace OpenCqrs.Exceptions
{
    public class ConcurrencyException : ApplicationException
    {
        public ConcurrencyException(Guid aggregateRootId, int expectedVersion, int actualVersion)
            : base(BuildErrorMesage(aggregateRootId, expectedVersion, actualVersion))
        {
        }

        private static string BuildErrorMesage(Guid aggregateRootId, int expectedVersion, int actualVersion)
        {
            return "Concurrency Exception" +
                   $" | AggregateRootId: {aggregateRootId.ToString()}" +
                   $" | Expected version: {expectedVersion}" +
                   $" | Actual version: {actualVersion}";
        }
    }
}