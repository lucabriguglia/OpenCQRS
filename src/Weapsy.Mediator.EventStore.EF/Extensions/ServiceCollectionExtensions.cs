using Microsoft.Extensions.DependencyInjection;

// ReSharper disable InconsistentNaming

namespace Weapsy.Mediator.EventStore.EF.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Weapsy EF EventStore
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddWeapsyEFEventStore(this IServiceCollection services)
        {
            // Use Scrutor to register services
            services.Scan(s => s
                .FromAssembliesOf(typeof(EventStore))
                .AddClasses()
                .AsImplementedInterfaces());
        }
    }
}