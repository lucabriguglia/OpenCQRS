using Microsoft.EntityFrameworkCore;

namespace Kledex.Store.EF.InMemory
{
    public class InMemoryDatabaseProvider : IDatabaseProvider
    {
        public DomainDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseInMemoryDatabase(connectionString);

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}