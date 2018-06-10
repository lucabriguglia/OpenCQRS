using System;
using Microsoft.Extensions.Options;
using Weapsy.Cqrs.Dependencies;
using Weapsy.Cqrs.EF.Configuration;

namespace Weapsy.Cqrs.EF
{
    public class DomainDbContextFactory : IDomainDbContextFactory
    {
        private readonly IResolver _resolver;
        private readonly string _eventStoreConnection;

        public DomainDbContextFactory(IResolver resolver, IOptions<DomainDbConfiguration> eventStoreConfiguration)
        {
            _resolver = resolver;
            _eventStoreConnection = eventStoreConfiguration.Value.ConnectionString;
        }

        public DomainDbContext CreateDbContext()
        {
            var dataProvider = _resolver.Resolve<IDatabaseProvider>();

            if (dataProvider == null)
                throw new ApplicationException("Domain database provider not found.");

            return dataProvider.CreateDbContext(_eventStoreConnection);
        }
    }
}
