using Kledex.Store.EF.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Kledex.Store.EF.SqlServer
{
    public class SqlServerDatabaseProvider : IDatabaseProvider
    {
        private readonly string _connectionString;

        public SqlServerDatabaseProvider(IOptions<DatabaseOptions> settings)
        {
            _connectionString = settings.Value.ConnectionString;
        }

        public DomainDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}