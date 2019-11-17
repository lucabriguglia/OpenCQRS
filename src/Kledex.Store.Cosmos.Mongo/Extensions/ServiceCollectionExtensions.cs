using System;
using Kledex.Domain;
using Kledex.Extensions;
using Kledex.Store.Cosmos.Mongo.Documents.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.Cosmos.Mongo.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddCosmosDbMongoDbProvider(this IKledexServiceBuilder builder)
        {
            return AddCosmosDbMongoDbProvider(builder, opt => { });
        }

        public static IKledexServiceBuilder AddCosmosDbMongoDbProvider(this IKledexServiceBuilder builder, Action<DomainDbOptions> setupAction)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            builder.Services.Configure(setupAction);

            builder.Services
                .AddTransient<IStoreProvider, StoreProvider>();

            builder.Services
                .AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>()
                .AddTransient<ICommandDocumentFactory, CommandDocumentFactory>()
                .AddTransient<IEventDocumentFactory, EventDocumentFactory>();

            return builder;
        }
    }
}
