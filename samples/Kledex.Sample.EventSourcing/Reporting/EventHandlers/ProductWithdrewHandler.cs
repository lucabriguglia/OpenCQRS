using System;
using System.Threading.Tasks;
using Kledex.Events;
using Kledex.Sample.EventSourcing.Domain.Events;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.EventSourcing.Domain.Commands.Handlers
{
    public class ProductWithdrewHandler : IEventHandlerAsync<ProductWithdrew>
    {
        private readonly ReportingDbContext _dbContext;

        public ProductWithdrewHandler(ReportingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task HandleAsync(ProductWithdrew @event)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == @event.AggregateRootId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {@event.AggregateRootId}");
            }

            product.Status = ProductStatus.Draft;

            await _dbContext.SaveChangesAsync();
        }
    }
}
