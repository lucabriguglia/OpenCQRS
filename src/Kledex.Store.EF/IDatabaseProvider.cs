namespace Kledex.Store.EF
{
    public interface IDatabaseProvider
    {
        DomainDbContext CreateDbContext();
    }
}
