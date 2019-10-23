using Kledex.Domain;

namespace Kledex.Sample.EventSourcing.Domain.Commands
{
    public class UpdateProduct : DomainCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
