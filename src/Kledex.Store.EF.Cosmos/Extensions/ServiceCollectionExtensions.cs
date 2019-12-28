using System;
using Kledex.Extensions;
using Kledex.Store.EF.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.EF.Cosmos.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddCosmosProvider(this IKledexServiceBuilder builder)
        {
            return AddCosmosProvider(builder, opt => { });
        }

        public static IKledexServiceBuilder AddCosmosProvider(this IKledexServiceBuilder builder, Action<CosmosOptions> configureOptions)
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

            builder.AddEFProvider();

            builder.Services.AddTransient<IDatabaseProvider, CosmosDatabaseProvider>();

            return builder;
        }
    }
}
