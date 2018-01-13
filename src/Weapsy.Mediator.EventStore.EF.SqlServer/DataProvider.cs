using Microsoft.EntityFrameworkCore;

namespace Weapsy.Mediator.EventStore.EF.SqlServer
{
    public class DataProvider : IDataProvider
    {
        public EventStoreDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventStoreDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new EventStoreDbContext(optionsBuilder.Options);
        }
    }
}