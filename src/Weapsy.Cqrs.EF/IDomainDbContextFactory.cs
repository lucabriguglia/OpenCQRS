namespace Weapsy.Cqrs.EF
{
    public interface IDomainDbContextFactory
    {
        DomainDbContext CreateDbContext();
    }
}
