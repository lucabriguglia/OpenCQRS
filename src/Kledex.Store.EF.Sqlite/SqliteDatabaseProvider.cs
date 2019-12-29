using Kledex.Store.EF.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kledex.Store.EF.Sqlite
{
    public class SqliteDatabaseProvider : IDatabaseProvider
    {
        private readonly DatabaseOptions _settings;

        public SqliteDatabaseProvider(IOptions<DatabaseOptions> settings)
        {
            _settings = settings.Value;
        }

        public DomainDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseSqlite(_settings.ConnectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}