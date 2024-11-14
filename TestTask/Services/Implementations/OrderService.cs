using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrder()
        {
            var newest_order = await _context.Orders.Where(o => o.Quantity > 1)
                                                    .OrderByDescending(o => o.CreatedAt)
                                                    .FirstOrDefaultAsync();

            return newest_order;
        }

        public async Task<List<Order>> GetOrders()
        {
            var orders = await _context.Orders.Where(o => o.User.Status == UserStatus.Active)
                                              .OrderByDescending(o => o.CreatedAt)
                                              .ToListAsync();
            return orders;
        }
    }
}
