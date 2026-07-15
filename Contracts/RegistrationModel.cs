using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Contracts
{
    public class RegistrationModel
    {
        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; } = string.Empty;
    }
}
