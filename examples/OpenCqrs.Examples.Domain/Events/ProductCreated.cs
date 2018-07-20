using System;
using OpenCqrs.Bus;
using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Events
{
    public class ProductCreated : DomainEvent
    {
        public string Title { get; set; }
    }
}
