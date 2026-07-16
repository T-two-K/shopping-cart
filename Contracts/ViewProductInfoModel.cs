using System.Text.Json.Serialization;

namespace ShoppingCart.Contracts
{
    public class ViewProductInfoModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; } = decimal.Zero;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
