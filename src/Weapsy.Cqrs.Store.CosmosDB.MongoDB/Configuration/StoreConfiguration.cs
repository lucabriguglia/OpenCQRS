namespace Weapsy.Cqrs.Store.CosmosDB.MongoDB.Configuration
{
    public class StoreConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string AggregateCollectionName { get; set; }
        public string CommandCollectionName { get; set; }
        public string EventCollectionName { get; set; }
    }
}
