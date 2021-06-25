using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Clients.Base;
using WebStore.Clients.Mapping;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Servcies;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient Client) : base(Client, "api/products") { }

        public IEnumerable<Section> GetSections() => Get<IEnumerable<SectionDTO>>($"{Address}/sections").FromDTO();

        public IEnumerable<Brand> GetBrands() => Get<IEnumerable<BrandDTO>>($"{Address}/brands").FromDTO();

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null) =>
            Post(Address, Filter ?? new ProductFilter())
               .Content
               .ReadFromJsonAsync<IEnumerable<ProductDTO>>()
               .Result
               .FromDTO();

        public Product GetProductById(int id) => Get<ProductDTO>($"{Address}/{id}").FromDTO();
    }
}
