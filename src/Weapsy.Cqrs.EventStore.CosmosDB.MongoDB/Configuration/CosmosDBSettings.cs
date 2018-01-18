namespace Weapsy.Cqrs.EventStore.CosmosDB.MongoDB.Configuration
{
    public class CosmosDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string AggregateCollectionName { get; set; }
        public string EventCollectionName { get; set; }
    }
}
