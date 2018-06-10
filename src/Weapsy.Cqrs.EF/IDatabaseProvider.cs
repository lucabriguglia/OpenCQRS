namespace Weapsy.Cqrs.EF
{
    public interface IDatabaseProvider
    {
        DomainDbContext CreateDbContext(string connectionString);
    }
}
