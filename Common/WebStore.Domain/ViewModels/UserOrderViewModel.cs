using System.Collections.Generic;
using WebStore.Domain.DTO;

namespace WebStore.Domain.ViewModels
{
    public class UserOrderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<UserOrderItemViewModel> Products { get; set; }
    }

    public record UserOrderItemViewModel(ProductDTO Product, decimal Price, int Count)
    {
        public decimal TotalPrice => Price * Count;
    }
}
