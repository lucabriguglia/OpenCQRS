using System;
using System.Threading.Tasks;
using Kledex.Sample.NoEventSourcing.Domain;
using Kledex.Sample.NoEventSourcing.Domain.Commands;
using Kledex.Sample.NoEventSourcing.Reporting.Products;
using Kledex.Sample.NoEventSourcing.Reporting.Products.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kledex.Sample.NoEventSourcing.Pages
{
    public class EditModel : PageModel
    {
        private readonly IDispatcher _dispatcher;

        public EditModel(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [BindProperty]
        public Product Product { get; set; }

        public ProductAggregateModel ProductAggregateModel { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Product = await _dispatcher.GetResultAsync<GetProduct, Product>(new GetProduct
            {
                ProductId = id
            });

            ProductAggregateModel = await _dispatcher.GetResultAsync<GetEvents, ProductAggregateModel>(new GetEvents
            {
                ProductId = id
            });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var command = new UpdateProduct
            {
                AggregateRootId = id,
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price
            };

            await _dispatcher.SendAsync<UpdateProduct, Product>(command);

            return RedirectToPage("./Edit", new { id });
        }
    }
}