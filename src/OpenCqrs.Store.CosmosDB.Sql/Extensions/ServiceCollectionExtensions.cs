using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Domain;
using OpenCqrs.Extensions;
using OpenCqrs.Store.CosmosDB.Sql.Configuration;
using OpenCqrs.Store.CosmosDB.Sql.Documents;
using OpenCqrs.Store.CosmosDB.Sql.Documents.Factories;
using OpenCqrs.Store.CosmosDB.Sql.Repositories;

namespace OpenCqrs.Store.CosmosDB.Sql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IOpenCqrsBuilder AddCosmosDbSqlProvider(this IOpenCqrsBuilder builder, IConfiguration configuration)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            builder.Services.Configure<DomainDbConfiguration>(configuration.GetSection("DomainDbConfiguration"));

            var endpoint = configuration.GetSection("DomainDbConfiguration:ServerEndpoint").Value;
            var key = configuration.GetSection("DomainDbConfiguration:AuthKey").Value;
            builder.Services.AddSingleton<IDocumentClient>(x => new DocumentClient(new Uri(endpoint), key));

            builder.Services.AddTransient<ICommandStore, CommandStore>();
            builder.Services.AddTransient<IEventStore, EventStore>();

            builder.Services.AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>();
            builder.Services.AddTransient<ICommandDocumentFactory, CommandDocumentFactory>();
            builder.Services.AddTransient<IEventDocumentFactory, EventDocumentFactory>();

            builder.Services.AddTransient<IDocumentRepository<AggregateDocument>, AggregateRepository>();
            builder.Services.AddTransient<IDocumentRepository<CommandDocument>, CommandRepository>();
            builder.Services.AddTransient<IDocumentRepository<EventDocument>, EventRepository>();

            return builder;
        }
    }
}
