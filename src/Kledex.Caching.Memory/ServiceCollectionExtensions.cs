using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Kledex.Caching.Memory
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddMemoryCacheProvider(this IKledexServiceBuilder builder)
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
