using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Domain;
using OpenCqrs.Store.CosmosDB.Sql.Configuration;
using OpenCqrs.Store.CosmosDB.Sql.Documents;
using OpenCqrs.Store.CosmosDB.Sql.Documents.Factories;
using OpenCqrs.Store.CosmosDB.Sql.Repositories;

namespace OpenCqrs.Store.CosmosDB.Sql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenCqrsCosmosDbSqlProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DomainDbConfiguration>(configuration.GetSection("DomainDbConfiguration"));

            var endpoint = configuration.GetSection("DomainDbConfiguration:ServerEndpoint").Value;
            var key = configuration.GetSection("DomainDbConfiguration:AuthKey").Value;
            services.AddSingleton<IDocumentClient>(x => new DocumentClient(new Uri(endpoint), key));

            services.AddTransient<ICommandStore, CommandStore>();
            services.AddTransient<IEventStore, EventStore>();

            services.AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>();
            services.AddTransient<ICommandDocumentFactory, CommandDocumentFactory>();
            services.AddTransient<IEventDocumentFactory, EventDocumentFactory>();

            services.AddTransient<IDocumentRepository<AggregateDocument>, AggregateRepository>();
            services.AddTransient<IDocumentRepository<CommandDocument>, CommandRepository>();
            services.AddTransient<IDocumentRepository<EventDocument>, EventRepository>();

            return services;
        }
    }
}
