using System;
using Kledex.Queries;
using Kledex.Sample.NoEventSourcing.Domain;

namespace Kledex.Sample.NoEventSourcing.Reporting
{
    public class GetProduct : IQuery<Product>
    {
        public Guid ProductId { get; set; }
    }
}
