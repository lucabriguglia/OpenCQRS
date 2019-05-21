using Microsoft.EntityFrameworkCore;

namespace OpenCqrs.Store.EF.Sqlite
{
    public class SqliteDatabaseProvider : IDatabaseProvider
    {
        public DomainDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}