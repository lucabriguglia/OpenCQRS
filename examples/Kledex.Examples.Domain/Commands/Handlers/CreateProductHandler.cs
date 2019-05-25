using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;

namespace Kledex.Examples.Domain.Commands.Handlers
{
    public class CreateProductHandler : IDomainCommandHandlerAsync<CreateProduct>
    {
        public async Task<IEnumerable<IDomainEvent>> HandleAsync(CreateProduct command)
        {
            await Task.CompletedTask;

            var product = new Product(command.AggregateRootId, command.Title);

            return product.Events;
        }
    }
}
