using System;
using Kledex.Domain;

namespace Kledex.Examples.Domain.Commands
{
    public class DeleteProduct : DomainCommand
    {
        public Guid ProductId { get; set; }
    }
}
