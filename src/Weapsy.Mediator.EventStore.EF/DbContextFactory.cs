using System;
using Microsoft.Extensions.Options;
using Weapsy.Mediator.Dependencies;
using Weapsy.Mediator.EventStore.EF.Configuration;

namespace Weapsy.Mediator.EventStore.EF
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly IResolver _resolver;
        private readonly string _eventStoreConnection;

        public DbContextFactory(IResolver resolver, IOptions<ConnectionStrings> connectionStringsOptions)
        {
            _resolver = resolver;
            _eventStoreConnection = connectionStringsOptions.Value.EventStoreConnection;
        }

        public MediatorDbContext CreateDbContext()
        {
            var dataProvider = _resolver.Resolve<IDataProvider>();

            if (dataProvider == null)
                throw new ApplicationException("Event store data provider not found.");

            return dataProvider.CreateDbContext(_eventStoreConnection);
        }
    }
}
