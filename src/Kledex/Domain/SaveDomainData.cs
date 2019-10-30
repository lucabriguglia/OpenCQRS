using System.Collections.Generic;

namespace Kledex.Domain
{
    public class SaveDomainData
    {
        public IDomainCommand Command { get; set; }
        public IList<IDomainEvent> Events { get; set; } = new List<IDomainEvent>();
        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
    }
}
