using System;
using Microsoft.AspNetCore.Mvc;

using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Filters;

namespace WebStore.Controllers
{
    [ActionDescription("Главный контроллер")]
    public class HomeController : Controller
    {
        [ActionDescription("Главное действие")]
        [AddHeader("Test", "Header value")]
        public IActionResult Index() => View();

        public IActionResult Throw() => throw new ApplicationException("Test error!");

        public IActionResult SecondAction(string id) => Content($"Action with value id:{id}");
    }
}
