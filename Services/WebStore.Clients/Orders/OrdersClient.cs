using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using WebStore.Clients.Base;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Servcies;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(HttpClient Client) : base(Client, "api/orders") { }

        public async Task<IEnumerable<Order>> GetUserOrders(string UserName)
        {
            var orders_dto = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{UserName}");
            return orders_dto.FromDTO();
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order_dto = await GetAsync<OrderDTO>($"{Address}/{id}");
            return order_dto.FromDTO();
        }

        public async Task<Order> CreateOrder(string UserName, CartViewModel Cart, OrderViewModel OrderModel)
        {
            var create_order_model = new CreateOrderDTO
            {
                Items = Cart.ToDTO(),
                Order = OrderModel
            };
            var response = await PostAsync($"{Address}/{UserName}", create_order_model);
            var order_dto = await response.Content.ReadFromJsonAsync<OrderDTO>();
            return order_dto.FromDTO();

        }
    }
}
