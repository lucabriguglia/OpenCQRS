using Kledex.Store.EF.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kledex.Store.EF.SqlServer
{
    public class SqlServerDatabaseProvider : IDatabaseProvider
    {
        private readonly DatabaseOptions _settings;

        public SqlServerDatabaseProvider(IOptions<DatabaseOptions> settings)
        {
            _settings = settings.Value;
        }

        public DomainDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseSqlServer(_settings.ConnectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}