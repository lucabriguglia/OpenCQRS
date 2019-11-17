using Kledex.Bus.ServiceBus.Factories;
using Kledex.Bus.ServiceBus.Queues;
using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kledex.Bus.ServiceBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the service bus provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IKledexServiceBuilder AddServiceBusProvider(this IKledexServiceBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services
                .AddTransient<IBusMessageDispatcher, BusMessageDispatcher>()
                .AddTransient<IBusProvider, BusProvider>()
                .AddTransient<IMessageFactory, MessageFactory>();

            return builder;
        }
    }
}
