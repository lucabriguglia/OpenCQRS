using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Store.EF.SqlServer
{
    public class SqlServerDatabaseProvider : IDatabaseProvider
    {
        public DomainDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseSqlServer(new SqlConnection(connectionString));

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}