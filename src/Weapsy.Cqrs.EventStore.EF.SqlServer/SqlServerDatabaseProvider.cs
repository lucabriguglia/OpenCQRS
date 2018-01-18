using Microsoft.EntityFrameworkCore;

namespace Weapsy.Cqrs.EventStore.EF.SqlServer
{
    public class SqlServerDatabaseProvider : IDatabaseProvider
    {
        public EventStoreDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventStoreDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new EventStoreDbContext(optionsBuilder.Options);
        }
    }
}