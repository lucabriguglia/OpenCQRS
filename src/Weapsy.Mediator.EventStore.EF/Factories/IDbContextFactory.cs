namespace Weapsy.Mediator.EventStore.EF.Factories
{
    public interface IDbContextFactory
    {
        MediatorDbContext CreateDbContext();
    }
}
