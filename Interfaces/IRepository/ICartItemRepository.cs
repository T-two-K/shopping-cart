using ShoppingCart.Models;

namespace ShoppingCart.Interfaces.IRepository
{
    public interface ICartItemRepository
    {
        public Task<List<CartItem>> GetAllByIdAsync(int cartId, int productId);
        public Task<bool> AddAsync(CartItem cartItem);
        public Task<bool> AddRangeAsync (List<CartItem> cartItems);
        public Task<bool> UpdateAsync(CartItem cartItem);
        public Task<bool> UpdataRangeAsync(List<CartItem> cartItems);
        public Task<bool> RemoveAsync(CartItem cartItem);
        public Task<bool> RemoveRangeAsync(List<CartItem> cartItems);
    }
}
