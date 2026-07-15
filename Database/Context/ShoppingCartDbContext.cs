using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.Database.Context
{
    public class ShoppingCartDbContext : DbContext
    {
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;

        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(e => {
                e.HasKey(p => p.Id);
                e.Property(p => p.Name).HasMaxLength(50).IsRequired();
                e.Property(p => p.Price).HasPrecision(18, 8).IsRequired();
                e.Property(p => p.Description).HasMaxLength(400);
            });

            modelBuilder.Entity<Cart>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Number).HasMaxLength(32).IsRequired();

                e.HasOne(c => c.User)
                    .WithOne(u => u.Cart)
                    .HasForeignKey<Cart>(c => c.UserId);
            });

            modelBuilder.Entity<CartItem>(e =>
            {
                e.HasKey(ci => new { ci.ProductId, ci.CartId });

                e.HasOne(ci => ci.Product).WithMany(p => p.CartItems).HasForeignKey(ci => ci.ProductId);

                e.HasOne(ci => ci.Cart).WithMany(c => c.CartItems).HasForeignKey(ci => ci.CartId);
            });
        }
    }
}
