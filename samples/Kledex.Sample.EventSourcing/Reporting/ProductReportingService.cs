using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kledex.Caching;
using Kledex.Sample.EventSourcing.Domain;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.EventSourcing.Reporting
{
    public class ProductReportingService : IProductReportingService
    {
        private readonly ReportingDbContext _dbContext;
        private readonly ICacheManager _cacheManager;

        public ProductReportingService(ReportingDbContext dbContext, ICacheManager cacheManager)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public async Task<IList<ProductEntity>> GetProducts()
        {
            return await _cacheManager.GetOrSetAsync(CacheKeys.ProductsCacheKey, async () =>
            {
                return await _dbContext.Products.Where(x => x.Status != ProductStatus.Deleted).ToListAsync();
            });
        }

        public async Task<IList<ProductEntity>> GetAllProducts()
        {
            return await _dbContext.Products.Where(x => x.Status != ProductStatus.Deleted).ToListAsync();
        }

        public async Task<ProductEntity> GetProduct(Guid productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                throw new ApplicationException($"Product not found. Id: {productId}");
            }

            return product;
        }
    }
}
