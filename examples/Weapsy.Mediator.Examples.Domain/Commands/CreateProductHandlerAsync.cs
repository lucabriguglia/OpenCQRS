using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Mediator.Domain;

namespace Weapsy.Mediator.Examples.Domain.Commands
{
    public class CreateProductHandlerAsync : IDomainCommandHandlerAsync<CreateProduct>
    {
        public async Task<IEnumerable<IDomainEvent>> HandleAsync(CreateProduct command)
        {
            await Task.CompletedTask;

            var product = new Product(command.AggregateRootId, command.Title);

            return product.Events;
        }
    }
}
