using Microsoft.EntityFrameworkCore;
using ShoppingCart.Database.Context;
using ShoppingCart.Interfaces.IRepository;
using ShoppingCart.Models;

namespace ShoppingCart.Database.Repositories
{
    public class ProductRepository(ShoppingCartDbContext context) : IProductRepository
    {
        private ShoppingCartDbContext _context = context;

        public async Task<bool> AddAsync(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var realReason = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(realReason);
            }

            return true;
        }

        public async Task<Product?> GetByIdAsync(int productId) =>
            await _context.Products.FindAsync(productId);

        public async Task<List<Product>> GetAllAsync() =>
            await _context.Products.ToListAsync();

        public async Task<bool> UpdateAsync(Product product)
        {
            var entry = _context.Entry(product);

            if (entry.State == EntityState.Detached)
                _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRangeAsync(List<Product> products)
        {
            _context.Products.UpdateRange(products);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeProductCount(List<CartItem> oldCartItems, List<CartItem> newCartItems)
        {
            List<Product> updatedProducts = new();
            List<Product> products = await GetAllAsync();

            foreach (var cartItem in oldCartItems)
            {
                Product? product = products.FirstOrDefault(p => p.Id == cartItem.ProductId);

                if (product == null)
                    continue;

                product.Count += cartItem.Quantity;
                updatedProducts.Add(product);
            }

            foreach (var cartItem in newCartItems)
            {
                Product? product = products.FirstOrDefault(p => p.Id == cartItem.ProductId);

                if (product == null)
                    continue;

                if (product.Count < cartItem.Quantity)
                    cartItem.Quantity = product.Count;

                product.Count -= cartItem.Quantity;

                Product? alreadyUpdatedProduct = updatedProducts.FirstOrDefault(p => p.Id == product.Id);

                if (alreadyUpdatedProduct != null)
                    updatedProducts.Remove(alreadyUpdatedProduct);

                updatedProducts.Add(product);
            }

            await UpdateRangeAsync(updatedProducts);

            return true;
        }
    }
}
