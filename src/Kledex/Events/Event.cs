using System.Collections.Generic;

namespace Kledex.Events
{
    public class Event : IEvent
    {
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }
}
