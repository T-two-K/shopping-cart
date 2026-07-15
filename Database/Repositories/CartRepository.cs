using Microsoft.EntityFrameworkCore;
using ShoppingCart.Database.Context;
using ShoppingCart.Interfaces.IRepository;
using ShoppingCart.Models;

namespace ShoppingCart.Database.Repositories
{
    public class CartRepository(ShoppingCartDbContext context) : ICartRepository
    {
        private ShoppingCartDbContext _context = context;

        public async Task<bool> AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Cart?> GetByUserIdAsync(int userId) =>
            await _context.Carts.Include(c => c.User)
                .Include(c => c.CartItems).ThenInclude(c => c.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

        public async Task<bool> UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
