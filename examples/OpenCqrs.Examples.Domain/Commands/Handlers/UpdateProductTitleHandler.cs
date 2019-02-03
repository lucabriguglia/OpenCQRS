using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenCqrs.Abstractions.Commands;
using OpenCqrs.Abstractions.Domain;

namespace OpenCqrs.Examples.Domain.Commands.Handlers
{
    public class UpdateProductTitleHandler : ICommandHandlerWithDomainEventsAsync<UpdateProductTitle>
    {
        private readonly IRepository<Product> _repository;

        public UpdateProductTitleHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(UpdateProductTitle command)
        {
            var product = await _repository.GetByIdAsync(command.AggregateRootId);

            if (product == null)
                throw new ApplicationException("Product not found.");

            product.UpdateTitle(command.Title);

            return product.Events;
        }
    }
}
