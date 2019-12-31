using Kledex.Caching.Redis.Configuration;
using Kledex.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;

namespace Kledex.Caching.Redis.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddRedisCache(this IKledexServiceBuilder builder, Action<RedisCacheOptions> configureOptions)
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

            var sp = builder.Services.BuildServiceProvider();
            var options = sp.GetService<IOptions<RedisCacheOptions>>().Value;

            builder.Services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(options.ConnectionString));
            builder.Services.AddSingleton<ICacheProvider>(x => new RedisCacheProvider(x.GetRequiredService<IConnectionMultiplexer>(), options.Db, options.AsyncState));

            return builder;
        }
    }
}
