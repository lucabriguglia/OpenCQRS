using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenCqrs.Store.EF.Cosmos.Configuration;

namespace OpenCqrs.Store.EF.Cosmos
{
    public class CosmosDatabaseProvider : IDatabaseProvider
    {
        private readonly CosmosDbOptions _settings;

        public CosmosDatabaseProvider(IOptions<CosmosDbOptions> settings)
        {
            _settings = settings.Value;
        }

        public DomainDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();

            optionsBuilder.UseCosmos(_settings.ServiceEndpoint,  _settings.AuthKey, _settings.DatabaseName);

            return new CosmosDomainDbContext(optionsBuilder.Options, _settings);
        }
    }
}