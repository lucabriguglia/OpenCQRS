namespace Kledex.Store.Cosmos.Mongo.Configuration
{
    public class DomainDbConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string AggregateCollectionName { get; set; }
        public string CommandCollectionName { get; set; }
        public string EventCollectionName { get; set; }
    }
}
