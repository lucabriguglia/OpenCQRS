using System;
using System.Threading.Tasks;
using Kledex.Events;
using Kledex.Sample.EventSourcing.Domain;
using Kledex.Sample.EventSourcing.Domain.Events;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Domain.Commands.Handlers
{
    public class ProductDeletedHandler : IEventHandlerAsync<ProductDeleted>
    {
        private readonly ReportingDbContext _dbContext;

        public ProductDeletedHandler(ReportingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(ProductDeleted @event)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == @event.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {@event.AggregateRootId}");
            }

            product.Status = ProductStatus.Deleted;

            await _dbContext.SaveChangesAsync();
        }
    }
}
