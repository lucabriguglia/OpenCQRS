using System;
using Kledex.Dependencies;
using Microsoft.Extensions.Configuration;

namespace Kledex.Store.EF
{
    public class DomainDbContextFactory : IDomainDbContextFactory
    {
        private readonly IResolver _resolver;
        private readonly string _connectionString;

        public DomainDbContextFactory(IResolver resolver, IConfiguration configuration)
        {
            _resolver = resolver;
            _connectionString = configuration.GetConnectionString(Consts.DomainStoreConnectionString);
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
