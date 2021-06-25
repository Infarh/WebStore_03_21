using Microsoft.AspNetCore.Mvc;
using WebStore.Clients.Mapping;
using WebStore.Domain;
using WebStore.Interfaces.Servcies;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _ProductData;

        public ProductsApiController(IProductData ProductData) => _ProductData = ProductData;

        [HttpGet("sections")]
        public IActionResult GetSections() => Ok(_ProductData.GetSections().ToDTO());

        [HttpGet("brands")]
        public IActionResult GetBrands() => Ok(_ProductData.GetBrands().ToDTO());

        [HttpPost]
        public IActionResult GetProducts(ProductFilter Filter = null) => Ok(_ProductData.GetProducts(Filter).ToDTO());

        [HttpGet("{id:int}")]
        public IActionResult GetProduct(int id) => Ok(_ProductData.GetProductById(id).ToDTO());
    }
}
