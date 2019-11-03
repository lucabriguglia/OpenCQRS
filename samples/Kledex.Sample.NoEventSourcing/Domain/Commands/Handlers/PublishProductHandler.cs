using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Domain;
using Microsoft.EntityFrameworkCore;
using Kledex.Sample.NoEventSourcing.Domain.Events;
using Kledex.Sample.NoEventSourcing.Data;
using Kledex.Commands;
using Kledex.Events;

namespace Kledex.Sample.NoEventSourcing.Domain.Commands.Handlers
{
    public class PublishProductHandler : ICommandHandlerAsync<PublishProduct>
    {
        private readonly SampleDbContext _dbContext;

        public PublishProductHandler(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CommandResponse> HandleAsync(PublishProduct command)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == command.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {command.AggregateRootId}");
            }

            product.Publish();

            await _dbContext.SaveChangesAsync();

            return new CommandResponse
            {
                Events = new List<IDomainEvent>()
                {
                    new ProductPublished
                    {
                        AggregateRootId = product.Id
                    }
                }
            };
        }
    }
}
