using System;
using Weapsy.Mediator.Queries;

namespace Weapsy.Mediator.Examples.Reporting.Queries
{
    public class GetProduct : IQuery
    {
        public Guid Id { get; set; }
    }
}
