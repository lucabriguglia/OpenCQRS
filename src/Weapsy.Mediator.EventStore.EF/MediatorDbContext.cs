using Microsoft.EntityFrameworkCore;
using Weapsy.Mediator.EventStore.EF.Entities;

namespace Weapsy.Mediator.EventStore.EF
{
    public class MediatorDbContext : DbContext
    {
        public MediatorDbContext(DbContextOptions<MediatorDbContext> options)
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
