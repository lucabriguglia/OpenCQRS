using System;
using System.Threading.Tasks;
using Weapsy.Mediator.Examples.Domain;
using Weapsy.Mediator.Examples.Domain.Commands;
using Weapsy.Mediator.Examples.Reporting;
using Weapsy.Mediator.Examples.Reporting.Queries;

namespace Weapsy.Mediator.Examples.Shared
{
    public static class GettingStarted
    {
        public static async Task<ProductViewModel> CreateProduct(IMediator mediator)
        {
            var productId = Guid.NewGuid();

            // Create a new product (first domain event created).
            // ProductCreatedHandlerAsync should created the view model.
            await mediator.SendAndPublishAsync<CreateProduct, Product>(new CreateProduct
            {
                AggregateRootId = productId,
                Title = "My brand new product"
            });

            // Update title (second domain event created).
            // ProductTitleUpdatedHandlerAsync should update the view model with the new title.
            await mediator.SendAndPublishAsync<UpdateProductTitle, Product>(new UpdateProductTitle
            {
                AggregateRootId = productId,
                Title = "Updated product title"
            });

            // Update title again (third domain event created).
            // ProductTitleUpdatedHandlerAsync should update the view model again with the new title.
            await mediator.SendAndPublishAsync<UpdateProductTitle, Product>(new UpdateProductTitle
            {
                AggregateRootId = productId,
                Title = "Yeah! Third title!"
            });

            // Get the view model that should return the title used in the last update.
            var product = await mediator.GetResultAsync<GetProduct, ProductViewModel>(new GetProduct
            {
                Id = productId
            });

            return product;
        }
    }
}
