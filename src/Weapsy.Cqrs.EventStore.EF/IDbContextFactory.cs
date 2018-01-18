namespace Weapsy.Cqrs.EventStore.EF
{
    public interface IDbContextFactory
    {
        EventStoreDbContext CreateDbContext();
    }
}
