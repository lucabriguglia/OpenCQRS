using System;
using Newtonsoft.Json;

namespace Weapsy.Mediator.EventStore.CosmosDB.Sql.Documents
{
    public class AggregateDocument
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
