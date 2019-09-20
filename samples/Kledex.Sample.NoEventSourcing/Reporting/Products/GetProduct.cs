using System;
using Kledex.Queries;

namespace Kledex.Sample.NoEventSourcing.Reporting.Products
{
    public class GetProduct : IQuery
    {
        public Guid ProductId { get; set; }
    }
}
