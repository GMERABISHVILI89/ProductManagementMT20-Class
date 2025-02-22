using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementMT20.Interfaces;
using ProductManagementMT20.Models;
using ProductManagementMT20.Models.VM;

namespace ProductManagementMT20.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _context;

        public OrderController(IOrderService orderService, ApplicationDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _context.Products.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel orderViewModel)
        {
            await _orderService.CreateOrderAsync(orderViewModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _context.Orders.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
            if (order == null) 
                return NotFound();

            ViewBag.Products = await _context.Products.ToListAsync();

            OrderViewModel viewModel = new OrderViewModel()
            {
                Id = order.Id,
                ProductIds = order.Products.Select(x => x.Id).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderViewModel orderViewModel)
        {
            if(id != orderViewModel.Id)
                return BadRequest();

            await _orderService.UpdateOrderAsync(id, orderViewModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id); 
            if(order == null)
                return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction("Index");
        }
    }
}
