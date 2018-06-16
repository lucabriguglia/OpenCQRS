using System;
using Microsoft.Extensions.Options;
using OpenCqrs.Dependencies;
using OpenCqrs.Store.EF.Configuration;

namespace OpenCqrs.Store.EF
{
    public class DomainDbContextFactory : IDomainDbContextFactory
    {
        private readonly IResolver _resolver;
        private readonly string _eventStoreConnection;

        public DomainDbContextFactory(IResolver resolver, IOptions<DomainDbConfiguration> domainDbConfiguration)
        {
            _resolver = resolver;
            _eventStoreConnection = domainDbConfiguration.Value.ConnectionString;
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
