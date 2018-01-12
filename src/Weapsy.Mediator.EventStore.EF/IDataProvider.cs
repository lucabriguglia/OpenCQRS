using Microsoft.Extensions.DependencyInjection;

namespace Weapsy.Mediator.EventStore.EF
{
    public interface IDataProvider
    {
        IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString);
        MediatorDbContext CreateDbContext(string connectionString);
    }
}
