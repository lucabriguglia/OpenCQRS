using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Store.EF.PostgreSql
{
    public class PostgreSqlDatabaseProvider : IDatabaseProvider
    {
        public DomainDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseNpgsql(new SqlConnection(connectionString));

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}