using System;
using Kledex.Extensions;
using Kledex.Store.EF.Cosmos.Configuration;
using Kledex.Store.EF.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kledex.Store.EF.Cosmos.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddCosmosStore(this IKledexServiceBuilder builder)
        {
            return AddCosmosStore(builder, opt => { });
        }

        public static IKledexServiceBuilder AddCosmosStore(this IKledexServiceBuilder builder, Action<CosmosDbOptions> configureOptions)
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

            var serviceProvider = builder.Services.BuildServiceProvider();
            var dbOptions = serviceProvider.GetService<IOptions<CosmosDbOptions>>().Value;

            builder.AddEFStore();

            builder.Services.AddDbContext<DomainDbContext>(options =>
                options.UseCosmos(dbOptions.ServiceEndpoint, dbOptions.AuthKey, dbOptions.DatabaseName));

            builder.Services.AddTransient<IDatabaseProvider, CosmosDatabaseProvider>();

            return builder;
        }
    }
}
