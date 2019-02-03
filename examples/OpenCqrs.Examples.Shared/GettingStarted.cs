using System;
using System.Threading.Tasks;
using OpenCqrs.Abstractions;
using OpenCqrs.Examples.Domain;
using OpenCqrs.Examples.Domain.Commands;
using OpenCqrs.Examples.Reporting;
using OpenCqrs.Examples.Reporting.Queries;

namespace OpenCqrs.Examples.Shared
{
    public static class GettingStarted
    {
        public static async Task<ProductViewModel> CreateProduct(IDispatcher dispatcher)
        {
            var productId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            const string source = "0";

            // Create a new product (first domain event created).
            // ProductCreatedHandlerAsync should created the view model.
            await dispatcher.SendAndPublishAsync<CreateProduct, Product>(new CreateProduct
            {
                AggregateRootId = productId,
                Title = "My brand new product",
                UserId = userId,
                Source = source
            });

            // Update title (second domain event created).
            // ProductTitleUpdatedHandlerAsync should update the view model with the new title.
            await dispatcher.SendAndPublishAsync<UpdateProductTitle, Product>(new UpdateProductTitle
            {
                AggregateRootId = productId,
                Title = "Updated product title",
                UserId = userId,
                Source = source,
                ExpectedVersion = 1
            });

            // Update title again (third domain event created).
            // ProductTitleUpdatedHandlerAsync should update the view model again with the new title.
            await dispatcher.SendAndPublishAsync<UpdateProductTitle, Product>(new UpdateProductTitle
            {
                AggregateRootId = productId,
                Title = "Yeah! Third title!",
                UserId = userId,
                Source = source,
                ExpectedVersion = 2
            });

            // Get the view model that should return the title used in the last update.
            var product = await dispatcher.GetResultAsync<GetProduct, ProductViewModel>(new GetProduct
            {
                Id = productId
            });

            return product;
        }
    }
}
