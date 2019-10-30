using System.Collections.Generic;

namespace Kledex.Events
{
    public interface IEvent
    {
        IDictionary<string, object> Properties { get; set; }
    }
}
