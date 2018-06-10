using System;

namespace Weapsy.Cqrs.EF.Entities
{
    public class AggregateEntity
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
    }
}
