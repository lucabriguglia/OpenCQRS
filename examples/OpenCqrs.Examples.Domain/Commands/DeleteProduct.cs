using System;
using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Examples.Domain.Commands
{
    public class DeleteProduct : DomainCommand
    {
        public Guid ProductId { get; set; }
    }
}
