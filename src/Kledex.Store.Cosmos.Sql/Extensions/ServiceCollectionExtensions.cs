using System;
using Kledex.Domain;
using Kledex.Extensions;
using Kledex.Store.Cosmos.Sql.Configuration;
using Kledex.Store.Cosmos.Sql.Documents;
using Kledex.Store.Cosmos.Sql.Documents.Factories;
using Kledex.Store.Cosmos.Sql.Repositories;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kledex.Store.Cosmos.Sql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKledexServiceBuilder AddCosmosDbSqlProvider(this IKledexServiceBuilder builder, IConfiguration configuration)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            builder.Services.Configure<DomainDbConfiguration>(configuration.GetSection("DomainDbConfiguration"));

            var endpoint = configuration.GetSection("DomainDbConfiguration:ServerEndpoint").Value;
            var key = configuration.GetSection("DomainDbConfiguration:AuthKey").Value;
            builder.Services.AddSingleton<IDocumentClient>(x => new DocumentClient(new Uri(endpoint), key));

            builder.Services
                .AddTransient<IDomainStore, DomainStore>();

            builder.Services
                .AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>()
                .AddTransient<ICommandDocumentFactory, CommandDocumentFactory>()
                .AddTransient<IEventDocumentFactory, EventDocumentFactory>();

            builder.Services
                .AddTransient<IDocumentRepository<AggregateDocument>, AggregateRepository>()
                .AddTransient<IDocumentRepository<CommandDocument>, CommandRepository>()
                .AddTransient<IDocumentRepository<EventDocument>, EventRepository>();

            return builder;
        }
    }
}
