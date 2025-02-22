using Microsoft.EntityFrameworkCore;
using ProductManagementMT20.Interfaces;
using ProductManagementMT20.Models;
using ProductManagementMT20.Models.Entities;
using ProductManagementMT20.Models.VM;

namespace ProductManagementMT20.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(OrderViewModel orderViewModel)
        {
            var order = new Order()
            {
                Products = new List<Product>()
            };

            foreach (var productId in orderViewModel.ProductIds)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                if(product != null) 
                    order.Products.Add(product);
            }

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var orderToDelete = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

            _context.Orders.Remove(orderToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(x => x.Products)
                .ToListAsync();
        }

        public async Task UpdateOrderAsync(int orderId, OrderViewModel orderViewModel)
        {
            var order = await _context.Orders
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.Id == orderId);

            var products = new List<Product>();
            foreach (var productId in orderViewModel.ProductIds)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                if( product != null) 
                    products.Add(product);
            }

            order.Products = products;
            await _context.SaveChangesAsync();
        }
    }
}
