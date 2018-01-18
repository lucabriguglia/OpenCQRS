using Microsoft.EntityFrameworkCore;

namespace Weapsy.Cqrs.EventStore.EF.PostgreSql
{
    public class PostgreSqlDatabaseProvider : IDatabaseProvider
    {
        public EventStoreDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventStoreDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new EventStoreDbContext(optionsBuilder.Options);
        }
    }
}