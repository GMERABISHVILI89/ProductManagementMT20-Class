using ProductManagementMT20.Models.Entities;
using ProductManagementMT20.Models.VM;

namespace ProductManagementMT20.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(OrderViewModel orderViewModel);
        Task<List<Order>> GetOrdersAsync();

        Task UpdateOrderAsync(int orderId, OrderViewModel orderViewModel);

        Task DeleteOrderAsync(int orderId);
    }
}
