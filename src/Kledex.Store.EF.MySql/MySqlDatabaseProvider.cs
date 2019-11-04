using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Store.EF.MySql
{
    public class MySqlDatabaseProvider : IDatabaseProvider
    {
        public DomainDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseMySQL(new SqlConnection(connectionString));

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}