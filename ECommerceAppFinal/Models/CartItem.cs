public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }

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
