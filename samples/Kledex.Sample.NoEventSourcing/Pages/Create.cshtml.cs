using System.Threading.Tasks;
using Kledex.Sample.NoEventSourcing.Domain;
using Kledex.Sample.NoEventSourcing.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kledex.Sample.NoEventSourcing.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IDispatcher _dispatcher;

        public CreateModel(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var command = new CreateProduct
            {
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price
            };

            await _dispatcher.SendAsync(command);

            return RedirectToPage("/List");
        }
    }
}