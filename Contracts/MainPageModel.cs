using ShoppingCart.Models;
using System.Text.Json.Serialization;

namespace ShoppingCart.Contracts
{
    public class MainPageModel
    {
        [JsonPropertyName("products")]
        public List<Product> Products { get; set; } = new();

        [JsonPropertyName("cart")]
        public Cart Cart { get; set; } = new();
    }
}