using System.Collections.Generic;

namespace Kledex.Domain
{
    public class HandlerResponse
    {
        public IList<IDomainEvent> Events { get; set; } = new List<IDomainEvent>();
        public IList<object> Properties { get; set; } = new List<object>();
    }
}
