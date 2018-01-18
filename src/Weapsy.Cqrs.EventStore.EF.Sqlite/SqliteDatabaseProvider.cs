using Microsoft.EntityFrameworkCore;

namespace Weapsy.Cqrs.EventStore.EF.Sqlite
{
    public class SqliteDatabaseProvider : IDatabaseProvider
    {
        public EventStoreDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventStoreDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new EventStoreDbContext(optionsBuilder.Options);
        }
    }
}