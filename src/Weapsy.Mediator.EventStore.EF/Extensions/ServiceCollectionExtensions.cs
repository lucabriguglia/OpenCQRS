using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Mediator.EventStore.EF.Configuration;

// ReSharper disable InconsistentNaming

namespace Weapsy.Mediator.EventStore.EF.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyMediatorEF(this IServiceCollection services, IConfiguration configuration)
        {
            services.Scan(s => s
                .FromAssembliesOf(typeof(EventStoreDbContext))
                .AddClasses()
                .AsImplementedInterfaces());

            services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));

            return services;
        }
    }
}
