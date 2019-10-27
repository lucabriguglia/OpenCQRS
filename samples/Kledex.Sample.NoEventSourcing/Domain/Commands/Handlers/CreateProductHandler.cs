using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Sample.NoEventSourcing.Domain.Events;
using Kledex.Sample.NoEventSourcing.Data;

namespace Kledex.Sample.NoEventSourcing.Domain.Commands.Handlers
{
    public class CreateProductHandler : IDomainCommandHandlerAsync<CreateProduct, Product>
    {
        private readonly SampleDbContext _dbContext;

        public CreateProductHandler(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(CreateProduct command)
        {
            var product = new Product(command.Name, command.Description, command.Price);

            _dbContext.Products.Add(product);

            await _dbContext.SaveChangesAsync();

            return new List<IDomainEvent>()
            {
                new ProductCreated
                {
                    AggregateRootId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price
                }
            };
        }
    }
}
