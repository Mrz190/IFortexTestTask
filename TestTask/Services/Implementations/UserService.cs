using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser()
        {
            var user = await _context.Users
                .Select(u => new
                {
                    User = u,
                    TotalPrice = u.Orders
                                  .Where(o => o.Status == OrderStatus.Delivered && o.CreatedAt.Year == 2003)
                                  .Sum(o => o.Price * o.Quantity) // Summarize the cost
                })
                .OrderByDescending(u => u.TotalPrice)
                .FirstOrDefaultAsync();
            return user?.User;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _context.Users.Where(p => p.Orders.Any(u => u.Status == OrderStatus.Paid && u.CreatedAt.Year == 2010))
                                            .ToListAsync();
            return users;
        }
    }
}
