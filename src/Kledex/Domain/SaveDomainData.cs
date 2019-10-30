using System.Collections.Generic;

namespace Kledex.Domain
{
    public class SaveDomainData
    {
        public IDomainCommand Command { get; set; }
        public IList<IDomainEvent> Events { get; set; } = new List<IDomainEvent>();
    }
}
