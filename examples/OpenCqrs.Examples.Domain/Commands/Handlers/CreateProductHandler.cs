using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCqrs.Abstractions.Commands;
using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Examples.Domain.Commands.Handlers
{
    public class CreateProductHandler : ICommandHandlerWithDomainEventsAsync<CreateProduct>
    {
        public async Task<IEnumerable<IDomainEvent>> HandleAsync(CreateProduct command)
        {
            await Task.CompletedTask;

            var product = new Product(command.AggregateRootId, command.Title);

            return product.Events;
        }
    }
}
