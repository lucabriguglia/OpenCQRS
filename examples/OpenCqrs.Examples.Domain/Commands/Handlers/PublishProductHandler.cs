using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCqrs.Abstractions.Commands;
using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Examples.Domain.Commands.Handlers
{
    public class PublishProductHandler : ICommandHandlerWithDomainEventsAsync<PublishProduct>
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
                throw new ApplicationException("Product not found.");

            product.Publish();

            return product.Events;
        }
    }
}
