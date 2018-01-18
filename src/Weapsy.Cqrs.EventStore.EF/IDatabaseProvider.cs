namespace Weapsy.Cqrs.EventStore.EF
{
    public interface IDatabaseProvider
    {
        EventStoreDbContext CreateDbContext(string connectionString);
    }
}
