using ShoppingCart.Models;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Contracts
{
    public enum PageAction { Add, Update }

    public class EditProductPageModel
    {
        public PageAction Action { get; set; } = PageAction.Add;
        public Product? Product { get; set; }

        [Required]
        public string ProductPriceString { get; set; } = string.Empty;
    }
}
