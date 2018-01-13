namespace Weapsy.Mediator.EventStore.EF
{
    public interface IDbContextFactory
    {
        EventStoreDbContext CreateDbContext();
    }
}
