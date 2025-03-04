using ECommerceAppFinal.Exceptions;

public class OrderService : IOrderService
{
    private List<Order> orders = new List<Order>();
    private static List<Order> orderhistory = new List<Order>();

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
        try
        {
            int orderId = orders.Count + 1;

            Order newOrder = new Order(orderId, userId, totalAmount, paymentMode);

            if (PaymentService.PaymentGateway(newOrder))
            {
                orders.Add(newOrder);
                orderhistory.Add(newOrder);
                WriteCentered($"Order #{orderId} placed successfully via {paymentMode}!");
                WriteCentered("");
                WriteCentered("Invoice Details:\n");
                newOrder.DisplayOrder();
                WriteCentered("");
            }
            else
            {
                throw new PaymentFailedException();
            }
        }
        catch(PaymentFailedException ex)
        {
            WriteCentered(ex.Message);
        }

    }

    public bool ViewOrders()
    {
        if (orders.Count == 0)
        {
            WriteCentered("No orders found.");
            return false;
        }
        WriteCentered("\n Orders:");
        foreach (var order in orders)
        {
            WriteCentered($"ID: {order.OrderId}, User: {order.UserId}, Amount: ${order.TotalAmount}, Status: {order.Status}, Payment: {order.PaymentMethod}");
        }
        return true;

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