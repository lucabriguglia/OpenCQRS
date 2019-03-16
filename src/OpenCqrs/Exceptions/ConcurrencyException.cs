using System;

namespace OpenCqrs.Exceptions
{
    public class ConcurrencyException : ApplicationException
    {
        public ConcurrencyException(Guid aggregateRootId, int expectedVersion, int actualVersion)
            : base(BuildErrorMessage(aggregateRootId, expectedVersion, actualVersion))
        {
        }

        private static string BuildErrorMessage(Guid aggregateRootId, int expectedVersion, int actualVersion)
        {
            return "Concurrency Exception" +
                   $" | AggregateRootId: {aggregateRootId.ToString()}" +
                   $" | Expected version: {expectedVersion}" +
                   $" | Actual version: {actualVersion}";
        }
    }
}