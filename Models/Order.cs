public class Order
{
    public int OrderId { get; private set; }
    public int UserId { get; private set; }
    public double TotalAmount { get; private set; }
    public OrderStatus Status { get; set; }
    public PaymentMode PaymentMethod { get; private set; }
    public DateTime OrderDate { get; private set; }

    public Order(int orderId, int userId, double totalAmount, PaymentMode paymentMethod)
    {
        OrderId = orderId;
        UserId = userId;
        TotalAmount = totalAmount;
        Status = OrderStatus.Pending;
        PaymentMethod = paymentMethod;
        OrderDate = DateTime.Now;
    }

    public void DisplayOrder()
    {
        Console.WriteLine($"Order ID: {OrderId} | User ID: {UserId} | Amount: ${TotalAmount} | Status: {Status} | Payment: {PaymentMethod} | Date: {OrderDate}");
    }
}