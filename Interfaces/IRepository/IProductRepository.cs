using ShoppingCart.Models;

namespace ShoppingCart.Interfaces.IRepository
{
    public interface IProductRepository 
    {
        public Task<Product?> GetByIdAsync(int productId);
        public Task<List<Product>> GetAllAsync();
        public Task<bool> AddAsync(Product product);
        public Task<bool> UpdateAsync(Product product);
        public Task<bool> UpdateRangeAsync(List<Product> products);
        public Task<bool> RemoveAsync(Product product);
        public Task<bool> ChangeProductCount(List<CartItem> oldCartItems, List<CartItem> newCartItems);
    }
}