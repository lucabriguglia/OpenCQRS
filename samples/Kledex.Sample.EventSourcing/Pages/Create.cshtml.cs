using System.Threading.Tasks;
using Kledex.Sample.EventSourcing.Domain;
using Kledex.Sample.EventSourcing.Domain.Commands;
using Kledex.Sample.EventSourcing.Reporting.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kledex.Sample.EventSourcing.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IDispatcher _dispatcher;
        private readonly IProductService _productService;

        public CreateModel(IDispatcher dispatcher, IProductService productService)
        {
            _dispatcher = dispatcher;
            _productService = productService;
        }

        [BindProperty]
        public ProductEntity Product { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var command = new CreateProduct
            {
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price
            };

            // Option 1 - The dispatcher will automatically resolve the command handler (ICommandHandlerAsync<CreateProduct>)
            //await _dispatcher.SendAsync(command);

            // Option 2 - Use your custom command handler or service
            await _dispatcher.SendAsync(command, () => _productService.CreateProductAsync(command));

            return RedirectToPage("/List");
        }
    }
}