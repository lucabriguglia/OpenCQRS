using System;
using Kledex.Extensions;
using Kledex.Store.EF.Cosmos.Configuration;
using Kledex.Store.EF.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.EF.Cosmos.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddCosmosStore(this IKledexServiceBuilder builder)
        {
            return AddCosmosStore(builder, opt => { });
        }

        public static IKledexServiceBuilder AddCosmosStore(this IKledexServiceBuilder builder, Action<CosmosDatabaseOptions> configureOptions)
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

            builder.AddEFStore();

            builder.Services.AddTransient<IDatabaseProvider, CosmosDatabaseProvider>();

            return builder;
        }
    }
}
