using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Weapsy.Mediator.EventStore.EF.Extensions;

namespace Weapsy.Mediator.EventStore.EF.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyMediatorEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWeapsyMediatorEF(configuration);

            var connectionString = configuration.GetConnectionString("EventStoreConnection");

            services.AddDbContext<MediatorDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddTransient<IDataProvider, SqlServerDataProvider>();

            return services;
        }
    }
}
