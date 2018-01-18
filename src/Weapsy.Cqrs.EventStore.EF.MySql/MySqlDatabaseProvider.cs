using Microsoft.EntityFrameworkCore;

namespace Weapsy.Cqrs.EventStore.EF.MySql
{
    public class MySqlDatabaseProvider : IDatabaseProvider
    {
        public EventStoreDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventStoreDbContext>();
            optionsBuilder.UseMySQL(connectionString);

            return new EventStoreDbContext(optionsBuilder.Options);
        }
    }
}