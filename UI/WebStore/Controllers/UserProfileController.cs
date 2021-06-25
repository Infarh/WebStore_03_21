using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        public IActionResult Index() => View();

        public async Task<IActionResult> Orders([FromServices] IOrderService OrderService, [FromServices] IProductData ProductData)
        {
            if (await OrderService.GetUserOrders(User.Identity!.Name) is not { } orders) return NotFound();

            var products = ProductData
               .GetProducts(new ProductFilter { Ids = orders.SelectMany(o => o.Items).Select(i => i.ProductId).Distinct().ToArray() })
               .Products.ToDictionary(p => p.Id);

            return View(orders.Select(o => new UserOrderViewModel
            {
                Id = o.Id,
                Name = o.Name,
                Phone = o.Phone,
                Address = o.Address,
                TotalPrice = o.Items.Sum(item => item.FromDTO().TotalItemPrice),
                Products = o.Items
                   .Where(i => products.ContainsKey(i.ProductId))
                   .Select(i => new UserOrderItemViewModel(products[i.ProductId], i.Price, i.Quantity))
            }));
        }
    }
}
