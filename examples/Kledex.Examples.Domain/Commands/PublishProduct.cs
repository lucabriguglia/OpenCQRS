using System;
using Kledex.Domain;

namespace Kledex.Examples.Domain.Commands
{
    public class PublishProduct : DomainCommand
    {
        public Guid ProductId { get; set; }
    }
}
