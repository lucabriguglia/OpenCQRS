using System;
using System.Threading.Tasks;
using Kledex.Models;
using Kledex.Sample.NoEventSourcing.Domain;
using Kledex.Sample.NoEventSourcing.Domain.Commands;
using Kledex.Sample.NoEventSourcing.Reporting;
using Kledex.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kledex.Sample.NoEventSourcing.Pages
{
    public class EditModel : PageModel
    {
        private readonly IDispatcher _dispatcher;
        private readonly IDomainService _domainService;
        private readonly IProductReportingService _productService;

        public EditModel(IDispatcher dispatcher,
            IDomainService domainService,
            IProductReportingService productService)
        {
            _dispatcher = dispatcher;
            _domainService = domainService;
            _productService = productService;
        }

        [BindProperty]
        public Product Product { get; set; }

        public AggregateModel AggregateModel { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Product = await _productService.GetProduct(id);

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