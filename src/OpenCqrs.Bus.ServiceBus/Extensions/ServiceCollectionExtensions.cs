using Microsoft.Extensions.DependencyInjection;

// ReSharper disable InconsistentNaming

namespace OpenCqrs.Bus.ServiceBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenCqrsServiceBus(this IServiceCollection services)
        {
            services.AddTransient<IMessageSender, MessageSender>();

            return services;
        }
    }
}
