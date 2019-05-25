using System;
using Kledex.Exceptions;

namespace Kledex.Domain
{
    public class VersionService : IVersionService
    {
        /// <inheritdoc />
        /// <exception cref="T:Kledex.Exceptions.ConcurrencyException"></exception>
        public int GetNextVersion(Guid aggregateRootId, int currentVersion, int? expectedVersion)
        {
            if (expectedVersion.HasValue && expectedVersion.Value > 0 && expectedVersion.Value != currentVersion)
                throw new ConcurrencyException(aggregateRootId, expectedVersion.Value, currentVersion);

            return currentVersion + 1;
        }
    }
}