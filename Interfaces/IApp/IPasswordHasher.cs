namespace ShoppingCart.Interfaces.IApp
{
    public interface IPasswordHasher
    {
        public string CreateHash(string password);

        public bool CompareHashes(string currentPasswordHash, string password);
    }
}
