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

        public CreateModel(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
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

            await _dispatcher.SendAsync(command);

            var result = await _dispatcher.SendAsync<bool>(command);

            return RedirectToPage("/List");
        }
    }
}