using System.Text.Json.Serialization;

namespace ShoppingCart.Models
{
    public class Cart
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("number")]
        public string Number { get; set; } = string.Empty;
        [JsonPropertyName("cartItems")]
        public List<CartItem> CartItems { get; set; } = new();
        [JsonPropertyName("totalPrice")]
        public decimal TotalPrice => CartItems?.Sum(ci => ci.Quantity * ci.Product.Price) ?? 0;
        [JsonPropertyName("productCount")]
        public int ProductCount => CartItems?.Sum(ci => ci.Quantity) ?? 0;
        [JsonIgnore]
        public User User { get; set; } = null!; 
    }
}
