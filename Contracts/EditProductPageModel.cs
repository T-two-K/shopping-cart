using ShoppingCart.Models;

namespace ShoppingCart.Contracts
{
    public enum PageAction { Add, Update }
    public class EditProductPageModel
    {
        public PageAction Action { get; set; } = PageAction.Add;
        public Product? Product { get; set; }
    }
}
