using Kledex.Sample.NoEventSourcing.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Data
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Ignore(p => p.Events)
                .Ignore(p => p.Version)
                .ToTable("Products");
        }

        public DbSet<Product> Products { get; set; }
    }
}
