using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;
using Microsoft.EntityFrameworkCore;
using Kledex.Sample.NoEventSourcing.Domain.Events;
using Kledex.Sample.NoEventSourcing.Data;

namespace Kledex.Sample.NoEventSourcing.Domain.Commands.Handlers
{
    public class PublishProductHandler : IDomainCommandHandlerAsync<PublishProduct>
    {
        private readonly SampleDbContext _dbContext;

        public PublishProductHandler(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<IDomainEvent>> HandleAsync(PublishProduct command)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == command.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {command.AggregateRootId}");
            }

            product.Publish();

            await _dbContext.SaveChangesAsync();

            return new List<IDomainEvent>()
            {
                new ProductPublished
                {
                    AggregateRootId = product.Id
                }
            };
        }
    }
}
