public class OrderService : IOrderService
{
    private List<Order> orders = new List<Order>();
    public static List<Order> orderhistory = new List<Order>();

    private static void WriteCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.WriteLine(new string(' ', spaces) + text);
    }
     
    public OrderService()
    {
    }


    public void PlaceOrder(int userId, double totalAmount, PaymentMode paymentMode)
    {
        int orderId = orders.Count + 1;

        Order newOrder = new Order(orderId, userId, totalAmount, paymentMode);

        if (PaymentService.PaymentGateway(newOrder))
        {
            orders.Add(newOrder);
            orderhistory.Add(newOrder);
            WriteCentered($"Order #{orderId} placed successfully via {paymentMode}!");
        }
        else
        {
            WriteCentered("Payment failed! Order not placed.");
        }

    }

    public void ViewOrders()
    {
        if (orders.Count == 0)
        {
            WriteCentered("No orders found.");
            return;
        }

        WriteCentered("\n Orders:");
        foreach (var order in orders)
        {
            WriteCentered($"ID: {order.OrderId}, User: {order.UserId}, Amount: ${order.TotalAmount}, Status: {order.Status}, Payment: {order.PaymentMethod}");
        }
    }

    public void UpdateOrderStatus(int orderId)
    {
        var order = orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
        {
            WriteCentered("Order not found!");
            return;
        }

        order.Status = OrderStatus.Delivered;
        WriteCentered($"Order #{orderId} marked as Delivered!");
    }

}