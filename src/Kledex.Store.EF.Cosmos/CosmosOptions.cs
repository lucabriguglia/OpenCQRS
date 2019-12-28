using Microsoft.Azure.Cosmos;

namespace Kledex.Store.EF.Cosmos
{
    public class CosmosOptions
    {
        public string ServiceEndpoint { get; set; } = "https://localhost:8081";
        public string AuthKey { get; set; } = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public string DatabaseName { get; set; } = "DomainStore";
        public string AggregateContainerName { get; set; } = "Aggregates";
        public string CommandContainerName { get; set; } = "Commands";
        public string EventContainerName { get; set; } = "Events";
        public int OfferThroughput { get; set; } = 400;
        public ConsistencyLevel ConsistencyLevel { get; set; } = ConsistencyLevel.Session;
    }
}
