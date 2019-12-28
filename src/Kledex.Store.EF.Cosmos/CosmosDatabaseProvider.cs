using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kledex.Store.EF.Cosmos
{
    public class CosmosDatabaseProvider : IDatabaseProvider
    {
        private readonly CosmosOptions _settings;

        public CosmosDatabaseProvider(IOptions<CosmosOptions> settings)
        {
            _settings = settings.Value;
        }

        public DomainDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();

            optionsBuilder.UseCosmos(_settings.ServiceEndpoint,  _settings.AuthKey, _settings.DatabaseName);

            return new CosmosDomainDbContext(optionsBuilder.Options, _settings);
        }
    }
}