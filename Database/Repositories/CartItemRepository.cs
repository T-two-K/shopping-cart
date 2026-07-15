using Microsoft.EntityFrameworkCore;
using ShoppingCart.Database.Context;
using ShoppingCart.Interfaces.IRepository;
using ShoppingCart.Models;

namespace ShoppingCart.Database.Repositories
{
    public class CartItemRepository(ShoppingCartDbContext context) : ICartItemRepository
    {
        private ShoppingCartDbContext _context = context;

        public async Task<bool> AddAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddRangeAsync(List<CartItem> cartItems)
        {
            await _context.CartItems.AddRangeAsync(cartItems);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CartItem>> GetAllByIdAsync(int cartId, int productId) =>
            await _context.CartItems.Where(ci => ci.CartId == cartId && ci.ProductId == productId).ToListAsync();

        public async Task<bool> RemoveAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveRangeAsync(List<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdataRangeAsync(List<CartItem> cartItems)
        {
            _context.CartItems.UpdateRange(cartItems);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
