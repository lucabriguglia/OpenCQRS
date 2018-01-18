using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.EventStore.EF.Extensions;

namespace Weapsy.Cqrs.EventStore.EF.MySql
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWeapsyCqrsEF(configuration);

            var connectionString = configuration.GetConnectionString(Constants.EventStoreConnection);

            services.AddDbContext<EventStoreDbContext>(options =>
                options.UseMySQL(connectionString));

            services.AddTransient<IDatabaseProvider, MySqlDatabaseProvider>();

            return services;
        }
    }
}
