using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Contracts;
using ShoppingCart.Interfaces.IApp;
using ShoppingCart.Interfaces.IRepository;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    //Добавил продукт в корзину - уменьшил его общее количество в бд (то есть у другого пользователя будет доступно меньше товара)
    [Authorize]
    public class ShopController(
        IDataManager dataManager) : Controller
    {
        private readonly IDataManager _dataManager = dataManager;

        [HttpGet]
        public async Task<IActionResult> MainPage()
        {
            Cart cart = new();
            List<Product> products = await _dataManager.ProductRepository.GetAllAsync();
            List<CartItem> cartItems = new();

            if (int.TryParse(User.FindFirst("UserId")!.Value, out int userId))
                cart = await _dataManager.CartRepository.GetByUserIdAsync(userId) ?? new();

            MainPageModel model = new()
            {
                Products = products,
                Cart = cart,
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditProductPage(int productId = -1)
        {
            EditProductPageModel model = new();

            if (productId == -1 || productId == 0)
            {
                model.Action = PageAction.Add;
            }
            else
            {
                model.Action = PageAction.Update;
                model.Product = await _dataManager.ProductRepository.GetByIdAsync(productId) ?? new();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditProductPage(EditProductPageModel model)
        {
            if (model.Product == null || !ModelState.IsValid)
            {
                ModelState.AddModelError("", "Не все обязательные поля были заполнены!");
                return View(model);
            }

            if (model.Action == PageAction.Add)
                await _dataManager.ProductRepository.AddAsync(model.Product);
            else
                await _dataManager.ProductRepository.UpdateAsync(model.Product);

            return View("MainPage");
        }

        [HttpPost]
        public async Task<JsonResult> ConfirmChanges([FromBody]List<CartItem> cartItems)
        {
            if (cartItems == null)
                return Json(new MainPageModel());

            int userId = GetUserIdFromClaim();

            MainPageModel model = await _dataManager.UpdateProductData(userId, cartItems);

            return Json(model);
        }

        private int GetUserIdFromClaim()
        {
            if (!int.TryParse(User.FindFirst("UserId")!.Value, out int userId))
                throw new InvalidOperationException("Пользователь не аутентифицирован.");
            else
                return userId;
        }
    }
}
