using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.EventStore.CosmosDB.MongoDB.Configuration;
using Weapsy.Cqrs.EventStore.CosmosDB.MongoDB.Documents.Factories;

namespace Weapsy.Cqrs.EventStore.CosmosDB.MongoDB.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EventStoreConfiguration>(configuration.GetSection("EventStoreConfiguration"));

            services.AddTransient<IEventStore, EventStore>();
            services.AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>();
            services.AddTransient<IEventDocumentFactory, EventDocumentFactory>();

            return services;
        }
    }
}
