using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Commands
{
    public class CreateProduct : DomainCommand
    {
        public string Title { get; set; }
    }
}
