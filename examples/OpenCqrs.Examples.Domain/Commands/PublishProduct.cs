using System;
using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Examples.Domain.Commands
{
    public class PublishProduct : DomainCommand
    {
        public Guid ProductId { get; set; }
    }
}
