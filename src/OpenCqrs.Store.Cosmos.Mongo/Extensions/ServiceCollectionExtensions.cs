using System;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Domain;
using OpenCqrs.Extensions;
using OpenCqrs.Store.Cosmos.Mongo.Configuration;
using OpenCqrs.Store.Cosmos.Mongo.Documents.Factories;

namespace OpenCqrs.Store.Cosmos.Mongo.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddCosmosMongoStore(this IKledexServiceBuilder builder)
        {
            return AddCosmosMongoStore(builder, opt => { });
        }

        public static IKledexServiceBuilder AddCosmosMongoStore(this IKledexServiceBuilder builder, Action<MongoOptions> setupAction)
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
                .AddTransient<IStoreProvider, MongoStoreProvider>();

            builder.Services
                .AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>()
                .AddTransient<ICommandDocumentFactory, CommandDocumentFactory>()
                .AddTransient<IEventDocumentFactory, EventDocumentFactory>();

            return builder;
        }
    }
}
