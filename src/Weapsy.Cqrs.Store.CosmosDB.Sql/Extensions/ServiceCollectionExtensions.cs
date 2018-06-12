using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.Store.CosmosDB.Sql.Configuration;
using Weapsy.Cqrs.Store.CosmosDB.Sql.Documents.Factories;
using Weapsy.Cqrs.Store.CosmosDB.Sql.Stores;

namespace Weapsy.Cqrs.Store.CosmosDB.Sql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsCosmosDbSqlStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StoreConfiguration>(configuration.GetSection("StoreConfiguration"));

            var endpoint = configuration.GetSection("StoreConfiguration:ServerEndpoint").Value;
            var key = configuration.GetSection("StoreConfiguration:AuthKey").Value;
            services.AddSingleton<IDocumentClient>(x => new DocumentClient(new Uri(endpoint), key));

            services.AddTransient<ICommandStore, CommandStore>();
            services.AddTransient<IEventStore, EventStore>();
            services.AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>();
            services.AddTransient<ICommandDocumentFactory, CommandDocumentFactory>();
            services.AddTransient<IEventDocumentFactory, EventDocumentFactory>();
            services.AddTransient(typeof(IDocumentDbRepository<>), typeof(DocumentDbRepository<>));

            return services;
        }
    }
}
