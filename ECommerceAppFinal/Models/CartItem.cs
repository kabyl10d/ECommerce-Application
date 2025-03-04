public class CartItem
{
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public double Price { get; private set; }
    public int Quantity { get; private set; }

    public CartItem(int productId, string productName, double price, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
    }

    public void UpdateQuantity(int newQuantity)
    {
        Quantity = newQuantity;
    }
}
