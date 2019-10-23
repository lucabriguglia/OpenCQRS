using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;

namespace Kledex.Sample.EventSourcing.Domain.Commands.Handlers
{
    public class PublishProductHandler : IDomainCommandHandlerAsync<PublishProduct>
    {
        private readonly IRepository<Product> _repository;

        public PublishProductHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(PublishProduct command)
        {
            var product = await _repository.GetByIdAsync(command.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {command.AggregateRootId}");
            }

            product.Publish();

            return product.Events;
        }
    }
}
