using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;

namespace Kledex.Sample.EventSourcing.Domain.Commands.Handlers
{
    public class DeleteProductHandler : IDomainCommandHandlerAsync<DeleteProduct>
    {
        private readonly IRepository<Product> _repository;

        public DeleteProductHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(DeleteProduct command)
        {
            var product = await _repository.GetByIdAsync(command.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {command.AggregateRootId}");
            }

            product.Delete();

            return product.Events;
        }
    }
}
