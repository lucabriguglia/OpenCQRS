using Kledex.Store.EF.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kledex.Store.EF.PostgreSql
{
    public class PostgreSqlDatabaseProvider : IDatabaseProvider
    {
        private readonly DatabaseOptions _settings;

        public PostgreSqlDatabaseProvider(IOptions<DatabaseOptions> settings)
        {
            _settings = settings.Value;
        }

        public DomainDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseNpgsql(_settings.ConnectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}