using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kledex.Caching.Memory.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddMemoryCache(this IKledexServiceBuilder builder)
        {
            return AddMemoryCache(builder, opt => { });
        }

        public static IKledexServiceBuilder AddMemoryCache(this IKledexServiceBuilder builder, Action<CacheOptions> configureOptions)
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

            builder.Services.AddTransient<ICacheProvider, MemoryCacheProvider>();

            return builder;
        }
    }
}
