using Microsoft.Extensions.DependencyInjection;
using Weapsy.Mediator.EventStore.EF.Configuration;

namespace Weapsy.Mediator.EventStore.EF
{
    public interface IDataProvider
    {
        DataProvider Provider { get; }
        IServiceCollection RegisterDbContext(IServiceCollection services, string connectionString);
        MediatorDbContext CreateDbContext(string connectionString);
    }
}
