using System;
using Kledex.Queries;

namespace Kledex.UI.Queries
{
    public class GetAggregateModel : IQuery
    {
        public Guid AggregateRootId { get; set; }
    }
}
