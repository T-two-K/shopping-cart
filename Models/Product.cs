using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShoppingCart.Models
{
    public class Product
    {
        
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [Required]
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("cartItems")]
        public List<CartItem>? CartItems { get; set; }
    }
}