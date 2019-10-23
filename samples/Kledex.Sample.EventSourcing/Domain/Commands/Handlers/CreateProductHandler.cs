using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;

namespace Kledex.Sample.EventSourcing.Domain.Commands.Handlers
{
    public class CreateProductHandler : IDomainCommandHandlerAsync<CreateProduct>
    {
        private readonly IRepository<Product> _repository;

        public CreateProductHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(CreateProduct command)
        {
            var product = new Product(command.AggregateRootId, command.Name, command.Description, command.Price);

            return await Task.FromResult(product.Events);
        }
    }
}
