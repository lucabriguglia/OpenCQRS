using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Sample.EventSourcing.Domain;
using Kledex.Sample.EventSourcing.Domain.Commands;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Kledex.Sample.EventSourcing.Reporting.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kledex.Sample.EventSourcing.Pages
{
    public class ListModel : PageModel
    {
        private readonly IDispatcher _dispatcher;

        public ListModel(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public IList<ProductEntity> Products { get; private set; }

        public async Task OnGetAsync()
        {
            var query = new GetProducts();
            Products = await _dispatcher.GetResultAsync(query);
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

        public async Task<IActionResult> OnPostPublishAsync(Guid id)
        {
            var command = new PublishProduct
            {
                AggregateRootId = id
            };

            await _dispatcher.SendAsync<PublishProduct, Product>(command);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostWithdrawAsync(Guid id)
        {
            var command = new WithdrawProduct
            {
                AggregateRootId = id
            };

            await _dispatcher.SendAsync<WithdrawProduct, Product>(command);

            return RedirectToPage();
        }
    }
}