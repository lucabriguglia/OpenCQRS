namespace Weapsy.Cqrs.EventStore.EF.Configuration
{
    public class EventStoreConfiguration
    {
        public string ConnectionString { get; set; }
        public string AggregateTableName { get; set; }
        public string EventTableName { get; set; }
    }
}
