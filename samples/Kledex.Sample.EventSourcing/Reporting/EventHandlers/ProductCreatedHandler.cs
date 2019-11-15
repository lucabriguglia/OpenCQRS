using System.Threading.Tasks;
using Kledex.Caching;
using Kledex.Events;
using Kledex.Sample.EventSourcing.Domain.Events;
using Kledex.Sample.EventSourcing.Reporting.Data;

namespace Kledex.Sample.EventSourcing.Reporting.EventHandlers
{
    public class ProductCreatedHandler : IEventHandlerAsync<ProductCreated>
    {
        private readonly ReportingDbContext _dbContext;
        private readonly ICacheManager _cacheManager;

        public ProductCreatedHandler(ReportingDbContext dbContext, ICacheManager cacheManager)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public async Task HandleAsync(ProductCreated @event)
        {
            _dbContext.Products.Add(new ProductEntity
            { 
                Id = @event.AggregateRootId,
                Name = @event.Name,
                Description = @event.Description,
                Price = @event.Price,
                Status = @event.Status
            });

            await _dbContext.SaveChangesAsync();

            await _cacheManager.RemoveAsync(CacheKeys.ProductsCacheKey);
        }
    }
}
