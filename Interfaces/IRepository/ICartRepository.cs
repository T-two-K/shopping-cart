using ShoppingCart.Models;

namespace ShoppingCart.Interfaces.IRepository
{
    public interface ICartRepository 
    {
        public Task<Cart?> GetByUserIdAsync(int userId);
        public Task<bool> AddAsync(Cart cart);
        public Task<bool> UpdateAsync(Cart cart);
        public Task<bool> RemoveAsync(Cart cart);
    }
}
