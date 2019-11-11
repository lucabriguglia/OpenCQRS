using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Caching.Memory
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddMemoryCacheProvider(this IKledexServiceBuilder builder)
        {
            builder.Services.AddTransient<ICacheProvider, MemoryCacheProvider>();

            return builder;
        }
    }
}
