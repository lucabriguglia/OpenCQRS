using Microsoft.EntityFrameworkCore;

namespace Kledex.Store.EF.InMemory
{
    public class InMemoryDatabaseProvider : IDatabaseProvider
    {
        public DomainDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName: "DomainDb");

            return new DomainDbContext(optionsBuilder.Options);
        }
    }
}