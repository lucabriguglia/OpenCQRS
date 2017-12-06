using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Weapsy.Mediator.EventStore.EF
{
    public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<MediatorDbContext>
    {
        public MediatorDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MediatorDbContext>();
            builder.UseSqlServer("UsedForMigrationsOnlyUntilClassLibraryBugIsFixed");

            return new MediatorDbContext(builder.Options);
        }
    }
}