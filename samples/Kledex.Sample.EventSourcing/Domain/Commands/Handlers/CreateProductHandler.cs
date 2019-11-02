using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Commands;
using Kledex.Events;

namespace Kledex.Sample.EventSourcing.Domain.Commands.Handlers
{
    public class CreateProductHandler : ICommandHandlerAsync<CreateProduct>
    {
        public async Task<IEnumerable<IEvent>> HandleAsync(CreateProduct command)
        {
            var product = new Product(command.AggregateRootId, command.Name, command.Description, command.Price);

            return await Task.FromResult(product.Events);
        }
    }
}
