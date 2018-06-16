using System;
using System.Threading.Tasks;
using OpenCqrs.Commands;
using OpenCqrs.Domain;

namespace OpenCqrs.Examples.Domain.Commands.Handlers
{
    public class UpdateProductTitleHandlerAsync : ICommandHandlerWithAggregateAsync<UpdateProductTitle>
    {
        private readonly IRepository<Product> _repository;

        public UpdateProductTitleHandlerAsync(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<IAggregateRoot> HandleAsync(UpdateProductTitle command)
        {
            var product = await _repository.GetByIdAsync(command.AggregateRootId);

            if (product == null)
                throw new ApplicationException("Product not found.");

            product.UpdateTitle(command.Title);

            return product;
        }
    }
}
