using ShoppingCart.Models;

namespace ShoppingCart.Interfaces.IRepository
{
    public interface IUserRepository 
    {
        public Task<User?> GetByLoginAsync(string login);
        public Task<User?> GetByIdAsync(int id);
        public Task<bool> AddAsync(User user);
    }
}
