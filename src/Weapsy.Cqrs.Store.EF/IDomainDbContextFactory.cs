namespace Weapsy.Cqrs.Store.EF
{
    public interface IDomainDbContextFactory
    {
        DomainDbContext CreateDbContext();
    }
}
