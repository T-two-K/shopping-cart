namespace ShoppingCart.Contracts
{
    public class ConfirmActionModel
    {
        public string ActionType { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int ProductCoun { get; set; } 
    }
}
