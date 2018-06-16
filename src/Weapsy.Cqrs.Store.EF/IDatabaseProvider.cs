namespace Weapsy.Cqrs.Store.EF
{
    public interface IDatabaseProvider
    {
        DomainDbContext CreateDbContext(string connectionString);
    }
}
