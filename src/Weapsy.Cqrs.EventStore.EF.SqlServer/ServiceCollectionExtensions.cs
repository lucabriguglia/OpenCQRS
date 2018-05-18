using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.EventStore.EF.Extensions;

namespace Weapsy.Cqrs.EventStore.EF.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsSqlServerEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWeapsyCqrsEF(configuration);

            var connectionString = configuration.GetSection(Constants.EventStoreConnectionString).Value;

            services.AddDbContext<EventStoreDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddTransient<IDatabaseProvider, SqlServerDatabaseProvider>();

            return services;
        }
    }
}
