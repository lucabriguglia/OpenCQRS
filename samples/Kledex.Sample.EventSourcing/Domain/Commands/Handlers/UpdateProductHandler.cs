using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;

namespace Kledex.Sample.EventSourcing.Domain.Commands.Handlers
{
    public class UpdateProductHandler : IDomainCommandHandlerAsync<UpdateProduct>
    {
        private readonly IRepository<Product> _repository;

        public UpdateProductHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(UpdateProduct command)
        {
            var product = await _repository.GetByIdAsync(command.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {command.AggregateRootId}");
            }

            product.Update(command.Name, command.Description, command.Price);

            return product.Events;
        }
    }
}
