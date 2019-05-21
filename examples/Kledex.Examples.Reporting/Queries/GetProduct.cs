using System;
using OpenCqrs.Queries;

namespace OpenCqrs.Examples.Reporting.Queries
{
    public class GetProduct : IQuery
    {
        public Guid Id { get; set; }
    }
}
