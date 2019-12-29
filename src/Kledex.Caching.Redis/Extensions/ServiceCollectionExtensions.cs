using Kledex.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;

namespace Kledex.Caching.Redis.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddRedisCache(this IKledexServiceBuilder builder, IConfiguration configuration, int db = -1, object asyncState = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var connectionString = configuration.GetConnectionString("KledexRedisCache");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("KledexRedisCache connection string not found.");
            }

            builder.Services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(connectionString));
            builder.Services.AddSingleton<ICacheProvider>(x => new RedisCacheProvider(x.GetRequiredService<IConnectionMultiplexer>(), db, asyncState));

            return builder;
        }
    }
}
