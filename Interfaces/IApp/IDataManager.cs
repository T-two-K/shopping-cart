using ShoppingCart.Contracts;
using ShoppingCart.Database.Repositories;
using ShoppingCart.Interfaces.IRepository;
using ShoppingCart.Models;

namespace ShoppingCart.Interfaces.IApp
{
    public interface IDataManager
    {
        public IUserRepository UserRepository { get; }
        public ICartRepository CartRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICartItemRepository CartItemRepository { get; }

        public Task<MainPageModel> UpdateProductData(int userId, List<CartItem> cartItems);
    }
}
