using System;
using System.Collections.Generic;

namespace Kledex.Domain
{
    public class DeleteStoreData
    {
        public Guid AggregateRootId { get; set; }
        public Guid CommandId { get; set; }
        public IEnumerable<Guid> EventIds { get; set; }
    }
}
