using System.Collections.Generic;

namespace Kledex.Bus
{
    public interface IBusMessage
    {
        IDictionary<string, object> Properties { get; set; }
    }
}
