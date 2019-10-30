using System.Collections.Generic;

namespace Kledex.Commands
{
    public class Command : ICommand
    {
        public bool? PublishEvents { get; set; }
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }
}
