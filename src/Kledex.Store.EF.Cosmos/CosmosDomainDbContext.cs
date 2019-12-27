using Kledex.Store.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Store.EF.Cosmos
{
    public class CosmosDomainDbContext : DomainDbContext
    {
        public CosmosDomainDbContext(DbContextOptions<DomainDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AggregateEntity>()
                .ToContainer("DomainAggregate");

            builder.Entity<CommandEntity>()
                .ToContainer("DomainCommand");

            builder.Entity<EventEntity>()
                .ToContainer("DomainEvent");
        }
    }
}
