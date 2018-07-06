using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OpenCqrs.Store.EF.Entities;

namespace OpenCqrs.Store.EF
{
    public interface IDomainDbContext
    {
        DbSet<AggregateEntity> Aggregates { get; set; }
        DbSet<CommandEntity> Commands { get; set; }
        DbSet<EventEntity> Events { get; set; }

        DatabaseFacade Database { get; }
    }
}