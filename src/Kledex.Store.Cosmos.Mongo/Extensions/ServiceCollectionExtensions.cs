using System;
using Kledex.Domain;
using Kledex.Extensions;
using Kledex.Store.Cosmos.Mongo.Configuration;
using Kledex.Store.Cosmos.Mongo.Documents.Factories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.Cosmos.Mongo.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddCosmosDbMongoDbProvider(this IKledexServiceBuilder builder, IConfiguration configuration)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            builder.Services
                .Configure<DomainDbConfiguration>(configuration.GetSection("DomainDbConfiguration"));

            builder.Services
                .AddTransient<IAggregateStore, AggregateStore>()
                .AddTransient<ICommandStore, CommandStore>()
                .AddTransient<IEventStore, EventStore>();

            builder.Services
                .AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>()
                .AddTransient<ICommandDocumentFactory, CommandDocumentFactory>()
                .AddTransient<IEventDocumentFactory, EventDocumentFactory>();

            return builder;
        }
    }
}
