using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Contracts
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Login { get; set; } = string.Empty;
    }
}
