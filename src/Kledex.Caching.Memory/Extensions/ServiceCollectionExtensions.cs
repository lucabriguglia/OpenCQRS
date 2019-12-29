using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kledex.Caching.Memory.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddMemoryCache(this IKledexServiceBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddTransient<ICacheProvider, MemoryCacheProvider>();

            return builder;
        }
    }
}
