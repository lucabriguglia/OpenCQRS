using System;
using System.Threading.Tasks;
using Kledex.Models;
using Kledex.Sample.EventSourcing.Domain.Commands;
using Kledex.Sample.EventSourcing.Reporting;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Kledex.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kledex.Sample.EventSourcing.Pages
{
    public class EditModel : PageModel
    {
        private readonly IDispatcher _dispatcher;
        private readonly IDomainService _domainService;
        private readonly IProductReportingService _productReportingService;

        public EditModel(IDispatcher dispatcher,
            IDomainService domainService,
            IProductReportingService productReportingService)
        {
            _dispatcher = dispatcher;
            _domainService = domainService;
            _productReportingService = productReportingService;
        }

        [BindProperty]
        public ProductEntity Product { get; set; }

        public AggregateModel AggregateModel { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Product = await _productReportingService.GetProduct(id);

            AggregateModel = await _domainService.GetAggregateAsync(id);

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

            await _dispatcher.SendAsync(command);

            return RedirectToPage("./Edit", new { id });
        }
    }
}