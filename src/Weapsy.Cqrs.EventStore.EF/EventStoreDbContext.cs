using Microsoft.EntityFrameworkCore;
using Weapsy.Cqrs.EventStore.EF.Entities;

namespace Weapsy.Cqrs.EventStore.EF
{
    public class EventStoreDbContext : DbContext
    {
        public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AggregateEntity>()
                .ToTable("DomainAggregate");

            builder.Entity<EventEntity>()
                .ToTable("DomainEvent")
                .HasKey(x => new { x.AggregateId, x.SequenceNumber });
        }

        public DbSet<AggregateEntity> Aggregates { get; set; }
        public DbSet<EventEntity> Events { get; set; }
    }
}
