using Microsoft.Extensions.DependencyInjection;

// ReSharper disable InconsistentNaming

namespace Weapsy.Cqrs.Bus.ServiceBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsServiceBus(this IServiceCollection services)
        {
            services.AddTransient<IMessageSender, MessageSender>();

            return services;
        }
    }
}
