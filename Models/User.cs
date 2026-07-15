using System.Security.Policy;
using System.Text.Json.Serialization;

namespace ShoppingCart.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("login")]
        public string Login { get; set; } = string.Empty;
        [JsonPropertyName("passwordHash")]
        public string PasswordHash { get; set; } = string.Empty;
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
        [JsonPropertyName("cart")]
        public Cart? Cart { get; set; }
    }
}
