using Microsoft.EntityFrameworkCore;
using ShoppingCart.Database.Context;
using ShoppingCart.Interfaces;
using ShoppingCart.Interfaces.IRepository;
using ShoppingCart.Models;

namespace ShoppingCart.Database.Repositories
{
    public class UserRepository(ShoppingCartDbContext context) : IUserRepository
    {
        private ShoppingCartDbContext _context = context;

        public async Task<User?> GetByLoginAsync(string login)
            => await _context.Users.Include(u => u.Cart).FirstOrDefaultAsync(u => u.Login == login);

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.FindAsync(id);

        public async Task<bool> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
