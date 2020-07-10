using System;
using System.Threading.Tasks;
using Kledex.Events;
using Kledex.Sample.EventSourcing.Domain;
using Kledex.Sample.EventSourcing.Domain.Events;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.EventSourcing.Reporting.EventHandlers
{
    public class ProductPublishedHandler : IEventHandlerAsync<ProductPublished>
    {
        private readonly ReportingDbContext _dbContext;

        public ProductPublishedHandler(ReportingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(ProductPublished @event)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == @event.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {@event.AggregateRootId}");
            }

            product.Status = ProductStatus.Published;

            await _dbContext.SaveChangesAsync();
        }
    }
}
