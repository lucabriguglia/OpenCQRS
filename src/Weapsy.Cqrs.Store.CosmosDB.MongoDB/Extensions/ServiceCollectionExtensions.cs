using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenCqrs.Domain;
using OpenCqrs.Store.CosmosDB.MongoDB.Configuration;
using OpenCqrs.Store.CosmosDB.MongoDB.Documents.Factories;

namespace OpenCqrs.Store.CosmosDB.MongoDB.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenCqrsCosmosDbMongoDbProvider(this IServiceCollection services, IConfiguration configuration)
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
