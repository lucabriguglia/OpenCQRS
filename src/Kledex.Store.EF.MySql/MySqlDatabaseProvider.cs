using Kledex.Store.EF.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kledex.Store.EF.MySql
{
    public class MySqlDatabaseProvider : IDatabaseProvider
    {
        private readonly DatabaseOptions _settings;

        public MySqlDatabaseProvider(IOptions<DatabaseOptions> settings)
        {
            _settings = settings.Value;
        }

        public DomainDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseMySQL(_settings.ConnectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}