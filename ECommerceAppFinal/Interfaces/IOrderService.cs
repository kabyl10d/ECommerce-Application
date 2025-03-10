public interface IOrderService
{
    bool PlaceOrder(int userId, List<CartItem> list, double totalAmount, PaymentMode paymentMode);
    bool ViewOrders();
    void UpdateOrderStatus(int orderId,int pid);
}
