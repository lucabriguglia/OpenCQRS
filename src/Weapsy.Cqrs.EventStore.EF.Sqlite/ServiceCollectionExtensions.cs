using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.EventStore.EF.Extensions;

namespace Weapsy.Cqrs.EventStore.EF.Sqlite
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsSqliteEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWeapsyCqrsEF(configuration);

            var connectionString = configuration.GetSection(Constants.EventStoreConnectionString).Value;

            services.AddDbContext<EventStoreDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddTransient<IDatabaseProvider, SqliteDatabaseProvider>();

            return services;
        }
    }
}
