using Microsoft.Azure.Documents;

namespace Kledex.Store.Cosmos.Sql
{
    public class DomainDbOptions
    {
        public string DatabaseId { get; set; } = "DomainStore";
        public string AggregateCollectionId { get; set; } = "Aggregates";
        public string CommandCollectionId { get; set; } = "Commands";
        public string EventCollectionId { get; set; } = "Events";
        public PartitionKey PartitionKey { get; set; }
        public int? OfferThroughput { get; set; }
        public ConsistencyLevel? ConsistencyLevel { get; set; }
    }
}
