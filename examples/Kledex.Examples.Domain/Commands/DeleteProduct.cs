using System;
using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Commands
{
    public class DeleteProduct : DomainCommand
    {
        public Guid ProductId { get; set; }
    }
}
