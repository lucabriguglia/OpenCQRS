using Microsoft.EntityFrameworkCore;

namespace Kledex.Store.EF.PostgreSql
{
    public class PostgreSqlDatabaseProvider : IDatabaseProvider
    {
        public DomainDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}