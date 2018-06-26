namespace OpenCqrs.Store.EF.Configuration
{
    public class DomainDbConfiguration
    {
        public string ConnectionString { get; set; }
        public string AggregateTableName { get; set; }
        public string CommandTableName { get; set; }
        public string EventTableName { get; set; }
    }
}
