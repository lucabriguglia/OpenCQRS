using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Store.EF.Sqlite
{
    public class SqliteDatabaseProvider : IDatabaseProvider
    {
        public DomainDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseSqlite(new SqlConnection(connectionString));

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}