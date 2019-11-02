using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Commands;
using Kledex.Domain;
using Kledex.Events;

namespace Kledex.Sample.EventSourcing.Domain.Commands.Handlers
{
    public class UpdateProductHandler : ICommandHandlerAsync<UpdateProduct>
    {
        private readonly IRepository<Product> _repository;

        public UpdateProductHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IEvent>> HandleAsync(UpdateProduct command)
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
