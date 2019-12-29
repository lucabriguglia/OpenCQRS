using Kledex.Store.EF.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kledex.Store.EF.PostgreSql
{
    public class PostgreSqlDatabaseProvider : IDatabaseProvider
    {
        private readonly string _connectionString;

        public PostgreSqlDatabaseProvider(IOptions<DatabaseOptions> settings)
        {
            _connectionString = settings.Value.ConnectionString;
        }

        public DomainDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseNpgsql(_connectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}