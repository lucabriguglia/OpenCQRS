using System;
using Kledex.Domain;

namespace Kledex.Samples.EventSourcing.Domain.Commands
{
    public class DeleteProduct : DomainCommand
    {
        public Guid ProductId { get; set; }
    }
}
