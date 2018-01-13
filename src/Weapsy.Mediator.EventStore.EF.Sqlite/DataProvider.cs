using Microsoft.EntityFrameworkCore;

namespace Weapsy.Mediator.EventStore.EF.Sqlite
{
    public class DataProvider : IDataProvider
    {
        public EventStoreDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventStoreDbContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new EventStoreDbContext(optionsBuilder.Options);
        }
    }
}