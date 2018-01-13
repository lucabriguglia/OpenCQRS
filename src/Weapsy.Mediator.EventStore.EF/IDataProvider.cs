namespace Weapsy.Mediator.EventStore.EF
{
    public interface IDataProvider
    {
        EventStoreDbContext CreateDbContext(string connectionString);
    }
}
