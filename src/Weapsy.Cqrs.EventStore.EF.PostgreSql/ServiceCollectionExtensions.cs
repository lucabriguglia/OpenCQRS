using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.EventStore.EF.Extensions;

namespace Weapsy.Cqrs.EventStore.EF.PostgreSql
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsPostgreSqlEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWeapsyCqrsEF(configuration);

            var connectionString = configuration.GetSection(Constants.EventStoreConnectionString).Value;

            services.AddDbContext<EventStoreDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddTransient<IDatabaseProvider, PostgreSqlDatabaseProvider>();

            return services;
        }
    }
}
