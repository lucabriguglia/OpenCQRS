using System;
using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Commands
{
    public class PublishProduct : DomainCommand
    {
        public Guid ProductId { get; set; }
    }
}
