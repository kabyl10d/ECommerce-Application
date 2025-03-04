public interface IOrderService
{
    void PlaceOrder(int userId, double totalAmount, PaymentMode paymentMode);
    bool ViewOrders();
    void UpdateOrderStatus(int orderId);
}
