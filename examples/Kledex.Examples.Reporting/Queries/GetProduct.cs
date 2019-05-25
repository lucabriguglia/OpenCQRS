using System;
using Kledex.Queries;

namespace Kledex.Examples.Reporting.Queries
{
    public class GetProduct : IQuery
    {
        public Guid Id { get; set; }
    }
}
