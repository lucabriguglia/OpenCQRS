using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.EventStore.CosmosDB.Sql.Configuration;
using Weapsy.Cqrs.EventStore.CosmosDB.Sql.Documents.Factories;

namespace Weapsy.Cqrs.EventStore.CosmosDB.Sql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CosmosDBSettings>(configuration.GetSection("CosmosDBSettings"));

            var endpoint = configuration.GetSection("CosmosDBSettings:ServerEndpoint").Value;
            var key = configuration.GetSection("CosmosDBSettings:AuthKey").Value;
            services.AddSingleton<IDocumentClient>(x => new DocumentClient(new Uri(endpoint), key));

            services.AddTransient<IEventStore, EventStore>();
            services.AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>();
            services.AddTransient<IEventDocumentFactory, EventDocumentFactory>();
            services.AddTransient(typeof(IDocumentDbRepository<>), typeof(DocumentDbRepository<>));

            return services;
        }
    }
}
