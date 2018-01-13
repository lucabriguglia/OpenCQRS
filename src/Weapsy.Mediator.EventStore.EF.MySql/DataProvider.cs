using Microsoft.EntityFrameworkCore;

namespace Weapsy.Mediator.EventStore.EF.MySql
{
    public class DataProvider : IDataProvider
    {
        public EventStoreDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventStoreDbContext>();
            optionsBuilder.UseMySQL(connectionString);

            return new EventStoreDbContext(optionsBuilder.Options);
        }
    }
}