using System;
using Newtonsoft.Json;

namespace Weapsy.Cqrs.EventStore.CosmosDB.Sql.Documents
{
    public class AggregateDocument
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
