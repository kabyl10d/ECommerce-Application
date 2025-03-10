public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }

    public string MerchName { get; set; }

    public OrderStatus Status { get; set; }

    public CartItem(int productId, string productName, double price, int quantity, string merchname)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
        MerchName = merchname;
    }

    public void UpdateQuantity(int newQuantity)
    {
        Quantity = newQuantity;
    }
}
