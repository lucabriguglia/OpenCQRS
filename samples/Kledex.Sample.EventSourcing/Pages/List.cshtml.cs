using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Sample.EventSourcing.Domain.Commands;
using Kledex.Sample.EventSourcing.Reporting;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kledex.Sample.EventSourcing.Pages
{
    public class ListModel : PageModel
    {
        private readonly IDispatcher _dispatcher;
        private readonly IProductReportingService _productReportingService;

        public ListModel(IDispatcher dispatcher, IProductReportingService productReportingService)
        {
            _dispatcher = dispatcher;
            _productReportingService = productReportingService;
        }

        public IList<ProductEntity> Products { get; private set; }

        public async Task OnGetAsync()
        {
            Products = await _productReportingService.GetProducts();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var command = new DeleteProduct
            {
                AggregateRootId = id
            };

            await _dispatcher.SendAsync(command);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostPublishAsync(Guid id)
        {
            var command = new PublishProduct
            {
                AggregateRootId = id
            };

            var result = await _dispatcher.SendAsync<bool>(command);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostWithdrawAsync(Guid id)
        {
            var command = new WithdrawProduct
            {
                AggregateRootId = id
            };

            await _dispatcher.SendAsync(command);

            return RedirectToPage();
        }
    }
}