using Microsoft.EntityFrameworkCore;

namespace Weapsy.Cqrs.Store.EF.MySql
{
    public class MySqlDatabaseProvider : IDatabaseProvider
    {
        public DomainDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseMySQL(connectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}