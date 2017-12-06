namespace Weapsy.Mediator.EventStore.EF
{
    public interface IDbContextFactory
    {
        MediatorDbContext CreateDbContext();
    }
}
