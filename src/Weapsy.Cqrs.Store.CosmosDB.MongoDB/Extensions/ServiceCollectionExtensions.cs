using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.Store.CosmosDB.MongoDB.Configuration;
using Weapsy.Cqrs.Store.CosmosDB.MongoDB.Documents.Factories;

namespace Weapsy.Cqrs.Store.CosmosDB.MongoDB.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeapsyCqrsCosmosDbMongoDbProvider(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DomainDbConfiguration>(configuration.GetSection("DomainDbConfiguration"));

            services.AddTransient<ICommandStore, CommandStore>();
            services.AddTransient<IEventStore, EventStore>();

            services.AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>();
            services.AddTransient<ICommandDocumentFactory, CommandDocumentFactory>();
            services.AddTransient<IEventDocumentFactory, EventDocumentFactory>();

            return services;
        }
    }
}
