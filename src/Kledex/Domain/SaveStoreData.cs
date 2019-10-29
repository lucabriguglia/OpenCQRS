using System.Collections.Generic;

namespace Kledex.Domain
{
    public class SaveStoreData
    {
        public IDomainCommand Command { get; set; }
        public IEnumerable<IDomainEvent> Events { get; set; }
        public IList<object> Properties { get; set; } = new List<object>();
    }
}
