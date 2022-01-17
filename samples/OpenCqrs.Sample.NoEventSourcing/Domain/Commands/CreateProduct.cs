using Kledex.Domain;

namespace Kledex.Sample.NoEventSourcing.Domain.Commands
{
    public class CreateProduct : DomainCommand<Product>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
