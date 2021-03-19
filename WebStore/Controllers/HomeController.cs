using Microsoft.AspNetCore.Mvc;

using WebStore.Infrastructure.Conventions;

namespace WebStore.Controllers
{
    [ActionDescription("Главный контроллер")]
    public class HomeController : Controller
    {
        [ActionDescription("Главное действие")]
        public IActionResult Index() => View();

        public IActionResult SecondAction(string id) => Content($"Action with value id:{id}");
    }
}
