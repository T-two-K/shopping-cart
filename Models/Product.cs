using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShoppingCart.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Количество товара не может быть отрицательным!")]
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Цена не может быть отрицательной!")]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("cartItems")]
        public List<CartItem>? CartItems { get; set; }
    }
}