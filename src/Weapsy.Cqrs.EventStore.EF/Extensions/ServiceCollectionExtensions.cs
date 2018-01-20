using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.EventStore.EF.Configuration;

// ReSharper disable InconsistentNaming

namespace Weapsy.Cqrs.EventStore.EF.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsEF(this IServiceCollection services, IConfiguration configuration)
        {
            services.Scan(s => s
                .FromAssembliesOf(typeof(EventStoreDbContext))
                .AddClasses()
                .AsImplementedInterfaces());

            services.Configure<EventStoreConfiguration>(configuration.GetSection("EventStoreConfiguration"));

            return services;
        }
    }
}
