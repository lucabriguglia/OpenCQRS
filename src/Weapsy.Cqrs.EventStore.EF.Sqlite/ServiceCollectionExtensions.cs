using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.EventStore.EF.Extensions;

namespace Weapsy.Cqrs.EventStore.EF.Sqlite
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsEventStore(this IServiceCollection services, IConfiguration configuration)
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
