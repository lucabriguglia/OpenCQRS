using Kledex.Store.EF.Cosmos.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kledex.Store.EF.Cosmos
{
    public class CosmosDatabaseProvider : IDatabaseProvider
    {
        private readonly CosmosDatabaseOptions _settings;

        public CosmosDatabaseProvider(IOptions<CosmosDatabaseOptions> settings)
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