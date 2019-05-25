using System;
using Kledex.Dependencies;
using Kledex.Store.EF.Configuration;
using Microsoft.Extensions.Options;

namespace Kledex.Store.EF
{
    public class DomainDbContextFactory : IDomainDbContextFactory
    {
        private readonly IResolver _resolver;
        private readonly string _connectionString;

        public DomainDbContextFactory(IResolver resolver, IOptions<DomainDbConfiguration> domainDbConfiguration)
        {
            _resolver = resolver;
            _connectionString = domainDbConfiguration.Value.ConnectionString;
        }

        public DomainDbContext CreateDbContext()
        {
            var dataProvider = _resolver.Resolve<IDatabaseProvider>();

            if (dataProvider == null)
                throw new ApplicationException("Domain database provider not found.");

            return dataProvider.CreateDbContext(_connectionString);
        }
    }
}
