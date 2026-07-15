using System.Text.Json.Serialization;

namespace ShoppingCart.Models
{
    public class CartItem
    {
        [JsonPropertyName("cartId")]
        public int CartId { get; set; }
        [JsonIgnore]
        public Cart Cart { get; set; } = null!;

        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; } = null!;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("discount")]
        public double Disscount { get; set; }
    }
}
