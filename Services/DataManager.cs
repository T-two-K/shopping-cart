using ShoppingCart.Contracts;
using ShoppingCart.Database.Context;
using ShoppingCart.Database.Repositories;
using ShoppingCart.Interfaces.IApp;
using ShoppingCart.Interfaces.IRepository;
using ShoppingCart.Models;

namespace ShoppingCart.Services
{
    public class DataManager(
        IUserRepository userRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        ICartItemRepository cartItemRepository,
        ShoppingCartDbContext context) : IDataManager
    {
        public IUserRepository UserRepository { get; } = userRepository;
        public ICartRepository CartRepository { get; } = cartRepository;
        public IProductRepository ProductRepository { get; } = productRepository;
        public ICartItemRepository CartItemRepository { get; } = cartItemRepository;

        private readonly ShoppingCartDbContext _context = context;

        public async Task<MainPageModel> UpdateProductData(int userId, List<CartItem> cartItems)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                Cart? userCart = await CartRepository.GetByUserIdAsync(userId);

                if (userCart == null)
                {
                    userCart = new Cart() { UserId = userId };
                    await CartRepository.AddAsync(userCart);
                }

                await ProductRepository.ChangeProductCount(userCart.CartItems, cartItems);

                User user = await UserRepository.GetByIdAsync(userId)
                    ?? throw new Exception("Пользователь не аутентифицирован!");

                List<Product>  products = await ProductRepository.GetAllAsync();

                await CartItemRepository.RemoveRangeAsync(userCart.CartItems);

                List<CartItem> newCartItems = cartItems.Where(ci => ci.Quantity > 0).Select(ci => new CartItem
                {
                    CartId = userCart.Id,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                }).ToList();

                await CartItemRepository.AddRangeAsync(newCartItems);

                Cart newCart = await CartRepository.GetByUserIdAsync(userId) ?? new();

                await transaction.CommitAsync();

                return new MainPageModel() { Cart = newCart, Products = products };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
