using Kledex.Events;
using System.Collections.Generic;

namespace Kledex.Commands
{
    public class CommandResponse
    {
        public IEnumerable<IEvent> Events { get; set; } = new List<IEvent>();
        public object Result { get; set; }
    }
}
