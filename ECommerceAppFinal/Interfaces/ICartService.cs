public interface ICartService
{
    bool AddToCart(int userId, Product product, int quantity);
    void RemoveFromCart(int userId, int productId);
    void ViewCart(int userId, OrderService o);
    void Checkout(int userId,OrderService o,List<CartItem> p);
}
