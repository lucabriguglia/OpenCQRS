using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Cqrs.Examples.Reporting.Queries
{
    public class GetProduct : IQuery
    {
        public Guid Id { get; set; }
    }
}
