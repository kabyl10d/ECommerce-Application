public interface IOrderService
{
    void PlaceOrder(int userId, double totalAmount, PaymentMode paymentMode);
    void ViewOrders();
    void UpdateOrderStatus(int orderId);
}
