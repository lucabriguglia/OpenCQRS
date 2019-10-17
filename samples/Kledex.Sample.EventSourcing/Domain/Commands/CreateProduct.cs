using Kledex.Domain;

namespace Kledex.Samples.EventSourcing.Domain.Commands
{
    public class CreateProduct : DomainCommand
    {
        public string Title { get; set; }
    }
}
