using System;
using Microsoft.Extensions.Options;
using Weapsy.Cqrs.Dependencies;
using Weapsy.Cqrs.EventStore.EF.Configuration;

namespace Weapsy.Cqrs.EventStore.EF
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IResolver _resolver;
        private readonly string _eventStoreConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactory"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="eventStoreConfiguration">The connection strings options.</param>
        public DbContextFactory(IResolver resolver, IOptions<EventStoreConfiguration> eventStoreConfiguration)
        {
            _resolver = resolver;
            _eventStoreConnection = eventStoreConfiguration.Value.ConnectionString;
        }

        public EventStoreDbContext CreateDbContext()
        {
            var dataProvider = _resolver.Resolve<IDatabaseProvider>();

            if (dataProvider == null)
                throw new ApplicationException("Event store database provider not found.");

            return dataProvider.CreateDbContext(_eventStoreConnection);
        }
    }
}
