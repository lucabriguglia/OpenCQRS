using Microsoft.EntityFrameworkCore;

namespace Weapsy.Mediator.EventStore.EF.PostgreSql
{
    public class DataProvider : IDataProvider
    {
        public EventStoreDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventStoreDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new EventStoreDbContext(optionsBuilder.Options);
        }
    }
}