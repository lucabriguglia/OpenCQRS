using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Weapsy.Mediator.EventStore.EF
{
    public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<EventStoreDbContext>
    {
        public EventStoreDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<EventStoreDbContext>();
            builder.UseSqlServer("UsedForMigrationsOnlyUntilClassLibraryBugIsFixed");

            return new EventStoreDbContext(builder.Options);
        }
    }
}