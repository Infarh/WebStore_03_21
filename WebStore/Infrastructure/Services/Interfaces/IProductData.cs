using System.Collections.Generic;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();
    }
}
