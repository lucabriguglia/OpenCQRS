namespace Kledex.Store.EF
{
    public interface IDomainDbContextFactory
    {
        DomainDbContext CreateDbContext();
    }
}
