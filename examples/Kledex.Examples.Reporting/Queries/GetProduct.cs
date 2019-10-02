using System;
using Kledex.Examples.Domain;
using Kledex.Queries;

namespace Kledex.Examples.Reporting.Queries
{
    public class GetProduct : IQuery<ProductViewModel>
    {
        public Guid Id { get; set; }
    }
}
