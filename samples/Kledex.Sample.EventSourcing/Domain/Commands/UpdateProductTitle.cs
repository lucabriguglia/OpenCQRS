using Kledex.Domain;

namespace Kledex.Samples.EventSourcing.Domain.Commands
{
    public class UpdateProductTitle : DomainCommand
    {
        public string Title { get; set; }
    }
}
