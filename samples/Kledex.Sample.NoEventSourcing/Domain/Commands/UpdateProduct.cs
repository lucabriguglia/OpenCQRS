using Kledex.Domain;

namespace Kledex.Sample.NoEventSourcing.Domain.Commands
{
    public class UpdateProduct : DomainCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
