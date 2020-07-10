using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Sample.EventSourcing.Reporting.Data;

namespace Kledex.Sample.EventSourcing.Reporting
{
    public interface IProductReportingService
    {
        Task<IList<ProductEntity>> GetProducts();
        Task<IList<ProductEntity>> GetAllProducts();
        Task<ProductEntity> GetProduct(Guid productId);
    }
}
