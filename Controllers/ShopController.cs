using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Contracts;
using ShoppingCart.Interfaces.IApp;
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
        public async Task<IActionResult> ViewProductInfo(int id)
        {
            Product? product = await _dataManager.ProductRepository.GetByIdAsync(id);

            if (product == null)
                return await MainPage();

            ViewProductInfoModel model = new()
            {
                Name = product.Name,
                Price = product.Price,
                Count = product.Count,
                Description = (product.Description ?? "Описание отсутствует").Trim()
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditProductPage(int id)
        {
            EditProductPageModel model = new();

            if (id == 0)
            {
                model.Action = PageAction.Add;
            }
            else
            {
                model.Action = PageAction.Update;
                model.Product = await _dataManager.ProductRepository.GetByIdAsync(id) ?? new();
                model.ProductPriceString = model.Product.Price.ToString();
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

            if (model.ProductPriceString.Contains('.'))
                model.ProductPriceString = model.ProductPriceString.Replace('.', ',');

            if (!decimal.TryParse(model.ProductPriceString, out decimal price))
            {
                ModelState.AddModelError("", "Цена имеет невалидное значение!");
                return View(model);
            }

            if (model.Action == PageAction.Add)
            {
                model.Product.Price = price;
                await _dataManager.ProductRepository.AddAsync(model.Product);
            }
            else
            {
                Product? existingProduct = await _dataManager.ProductRepository.GetByIdAsync(model.Product.Id);

                if (existingProduct == null)
                {
                    await _dataManager.ProductRepository.AddAsync(model.Product);
                    return RedirectToAction(nameof(MainPage));
                }

                existingProduct.Name = model.Product.Name;
                existingProduct.Description = model.Product.Description;
                existingProduct.Count = model.Product.Count;
                existingProduct.Price = price;

                await _dataManager.ProductRepository.UpdateAsync(existingProduct);
            }

            return RedirectToAction(nameof(MainPage));
        }

        [HttpPost]
        public async Task<JsonResult> ConfirmChanges([FromBody] List<CartItem> cartItems)
        {
            if (cartItems == null)
                return Json(new MainPageModel());

            List<Product> products = await _dataManager.ProductRepository.GetAllAsync();

            List<CartItem> validCartItems =
                cartItems.Where(ci => products.FirstOrDefault(p => p.Id == ci.ProductId) != null).ToList(); 

            int userId = GetUserIdFromClaim();

            MainPageModel model = await _dataManager.UpdateProductData(userId, validCartItems);

            return Json(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<List<Product>> DeleteProduct([FromBody]int productId)
        {
            Product? deletedProduct = await _dataManager.ProductRepository.GetByIdAsync(productId);

            if (deletedProduct == null)
                return await _dataManager.ProductRepository.GetAllAsync();

            await _dataManager.ProductRepository.RemoveAsync(deletedProduct);

            return await _dataManager.ProductRepository.GetAllAsync();
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
