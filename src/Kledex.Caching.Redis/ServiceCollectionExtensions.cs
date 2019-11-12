using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Caching.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddRedisCacheProvider(this IKledexServiceBuilder builder)
        {
            builder.Services.AddTransient<ICacheProvider, RedisCacheProvider>();

            return builder;
        }
    }
}
