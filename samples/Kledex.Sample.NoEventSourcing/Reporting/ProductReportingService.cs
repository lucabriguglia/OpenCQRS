using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kledex.Sample.NoEventSourcing.Data;
using Kledex.Sample.NoEventSourcing.Domain;
using Microsoft.EntityFrameworkCore;

namespace Kledex.Sample.NoEventSourcing.Reporting
{
    public class ProductReportingService : IProductReportingService
    {
        private readonly SampleDbContext _dbContext;

        public ProductReportingService(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Product>> GetProducts()
        {
            return await _dbContext.Products.Where(x => x.Status != ProductStatus.Deleted).ToListAsync();
        }

        public async Task<IList<Product>> GetAllProducts()
        {
            return await _dbContext.Products.Where(x => x.Status != ProductStatus.Deleted).ToListAsync();
        }

        public async Task<Product> GetProduct(Guid productId)
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
