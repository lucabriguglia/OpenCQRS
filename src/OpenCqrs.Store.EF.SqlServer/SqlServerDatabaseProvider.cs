using Microsoft.EntityFrameworkCore;

namespace OpenCqrs.Store.EF.SqlServer
{
    public class SqlServerDatabaseProvider : IDatabaseProvider
    {
        public DomainDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}