using Kledex.Bus.ServiceBus.Factories;
using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kledex.Bus.ServiceBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddServiceBus(this IKledexServiceBuilder builder)
        {
            return AddServiceBus(builder, opt => { });
        }

        /// <summary>
        /// Adds the service bus provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IKledexServiceBuilder AddServiceBus(this IKledexServiceBuilder builder, Action<BusOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            builder.Services.Configure(configureOptions);

            builder.Services
                .AddTransient<IBusMessageDispatcher, BusMessageDispatcher>()
                .AddTransient<IBusProvider, ServiceBusProvider>()
                .AddTransient<IMessageFactory, MessageFactory>();

            return builder;
        }
    }
}
