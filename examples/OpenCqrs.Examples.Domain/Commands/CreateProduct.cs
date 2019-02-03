using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Examples.Domain.Commands
{
    public class CreateProduct : DomainCommand
    {
        public string Title { get; set; }
    }
}
