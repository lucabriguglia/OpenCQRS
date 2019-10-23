using System;
using Kledex.Queries;
using Kledex.UI.Models;

namespace Kledex.UI.Queries
{
    public class GetAggregateModel : IQuery<AggregateModel>
    {
        public Guid AggregateRootId { get; set; }
    }
}
