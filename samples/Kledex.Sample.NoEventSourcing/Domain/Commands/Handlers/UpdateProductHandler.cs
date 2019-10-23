using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;
using Kledex.Sample.NoEventSourcing.Data;
using Kledex.Sample.NoEventSourcing.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Domain.Commands.Handlers
{
    public class UpdateProductHandler : IDomainCommandHandlerAsync<UpdateProduct>
    {
        private readonly SampleDbContext _dbContext;

        public UpdateProductHandler(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(UpdateProduct command)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == command.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {command.AggregateRootId}");
            }

            product.Update(command.Name, command.Description, command.Price);

            await _dbContext.SaveChangesAsync();

            return new List<IDomainEvent>()
            {
                new ProductUpdated
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
