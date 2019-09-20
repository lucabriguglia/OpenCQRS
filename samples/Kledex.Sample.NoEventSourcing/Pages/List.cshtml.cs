using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Sample.NoEventSourcing.Domain;
using Kledex.Sample.NoEventSourcing.Domain.Commands;
using Kledex.Sample.NoEventSourcing.Reporting.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kledex.Sample.NoEventSourcing.Pages
{
    public class ListModel : PageModel
    {
        private readonly IDispatcher _dispatcher;

        public ListModel(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public IList<Product> Products { get; private set; }

        public async Task OnGetAsync()
        {
            var query = new GetAllProducts();
            Products = await _dispatcher.GetResultAsync<GetAllProducts, IList<Product>>(query);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var command = new DeleteProduct
            {
                AggregateRootId = id
            };

            await _dispatcher.SendAsync<DeleteProduct, Product>(command);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var command = new PublishProduct
            {
                AggregateRootId = id
            };

            await _dispatcher.SendAsync<PublishProduct, Product>(command);

            return RedirectToPage();
        }
    }
}