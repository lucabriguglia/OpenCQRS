using System;
using System.Threading.Tasks;
using Kledex.Events;
using Kledex.Sample.EventSourcing.Domain.Events;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Domain.Commands.Handlers
{
    public class ProductUpdatedHandler : IEventHandlerAsync<ProductUpdated>
    {
        private readonly ReportingDbContext _dbContext;

        public ProductUpdatedHandler(ReportingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(ProductUpdated @event)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == @event.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {@event.AggregateRootId}");
            }

            product.Name = @event.Name;
            product.Description = @event.Description;
            product.Price = @event.Price;

            await _dbContext.SaveChangesAsync();
        }
    }
}
