using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Database.Context;
using ShoppingCart.Database.Repositories;
using ShoppingCart.Interfaces.IApp;
using ShoppingCart.Interfaces.IRepository;
using ShoppingCart.Services;

namespace ShoppingCart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddControllersWithViews();

                builder.Services.AddDbContext<ShoppingCartDbContext>(options => 
                    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? throw new Exception("Такой строки подключеиня нет!")));

                builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<ICartRepository, CartRepository>();
                builder.Services.AddScoped<IProductRepository, ProductRepository>();
                builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
                builder.Services.AddScoped<IDataManager, DataManager>();

                builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Account/Login";
                        options.AccessDeniedPath = "/Account/AccessDenied";
                        options.ExpireTimeSpan = TimeSpan.FromDays(7);
                        options.SlidingExpiration = true;
                    });

                builder.Services.AddAuthorization();

                builder.Services.AddHttpContextAccessor();

                var app = builder.Build();

                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");

                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}\nStack trace: {ex.StackTrace}");
            }
        }
    }
}
