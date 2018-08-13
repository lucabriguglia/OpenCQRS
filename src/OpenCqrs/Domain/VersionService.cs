using System;
using OpenCqrs.Exceptions;

namespace OpenCqrs.Domain
{
    public class VersionService : IVersionService
    {
        /// <summary>
        /// Gets the next version.
        /// </summary>
        /// <param name="aggregateRootId">The aggregate root identifier.</param>
        /// <param name="currentVersion">The current version.</param>
        /// <param name="expectedVersion">The expected version.</param>
        /// <returns></returns>
        /// <exception cref="OpenCqrs.Exceptions.ConcurrencyException"></exception>
        public int GetNextVersion(Guid aggregateRootId, int currentVersion, int? expectedVersion)
        {
            if (expectedVersion.HasValue && expectedVersion.Value > 0 && expectedVersion.Value != currentVersion)
                throw new ConcurrencyException(aggregateRootId, expectedVersion.Value, currentVersion);

            return currentVersion + 1;
        }
    }
}