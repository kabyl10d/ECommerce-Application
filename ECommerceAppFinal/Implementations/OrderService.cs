﻿using ECommerceAppFinal.Exceptions;

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

    private static string ReadCentered(string prompt)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = prompt.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.Write(new string(' ', spaces) + prompt);
        return Console.ReadLine();
    }
    public bool PlaceOrder(int userId, List<CartItem> prodlist, double totalAmount, PaymentMode paymentMode)
    {
        try
        {
            int orderId = orders.Count + 1;

            Order newOrder = new Order(orderId, userId, totalAmount, paymentMode);
            
            if (PaymentService.PaymentGateway(newOrder))
            {
                addr:
                string shippingAddress = ReadCentered("Enter shipping address: ");
                if (string.IsNullOrEmpty(shippingAddress))
                {
                    WriteCentered("Enter valid address.");
                    goto addr;
                }
                newOrder.ShippingAddress = shippingAddress;

                orders.Add(newOrder);
                orderhistory.Add(newOrder);
                WriteCentered($"Order #{orderId} placed successfully via {paymentMode}!");
                WriteCentered("");
                WriteCentered("Invoice Details:\n");
                //newOrder.DisplayOrder();
                string paymentMode2 = "";
                if (newOrder.PaymentMethod == PaymentMode.UPI)
                {
                    paymentMode2 = newOrder.UPIMethod.ToString();
                }
                else if (newOrder.PaymentMethod == PaymentMode.Card)
                {
                    paymentMode2 = newOrder.PaymentMethod.ToString();
                }
                WriteCentered($"ID: {newOrder.OrderId}, User: {newOrder.UserId}, Amount: ${newOrder.TotalAmount} ");
                WriteCentered($"Shipping Address: {newOrder.ShippingAddress}\n");
                WriteCentered("Products:");
                foreach (var p in prodlist)
                {
                    WriteCentered($"Product id: {p.ProductId}, Product: {p.ProductName}, Quantity: {p.Quantity}, Price: ${p.Price}, Status: {p.Status}");
                }
                WriteCentered("");
                WriteCentered($"Payment: {paymentMode2}");

                WriteCentered("");
                newOrder.ProdList = prodlist;
                return true;
            }
            else
            {
                WriteCentered("Payment failed!");
                return false;
            }
        }
        catch (PaymentFailedException ex)
        {
            WriteCentered(ex.Message);
            return false;
        }
    }

    public bool ViewOrders()
    {
        if (orders.Count == 0)
        {
            Console.Clear();
            WriteCentered("No orders found.");
            return false;
        }
        WriteCentered("Orders:\n");
        foreach (var order in orders)
        {
            string paymentMode2 = "";
            if (order.PaymentMethod == PaymentMode.UPI)
            {
                paymentMode2 = order.UPIMethod.ToString();
            }
            else if (order.PaymentMethod == PaymentMode.Card)
            {
                paymentMode2 = order.PaymentMethod.ToString();
            }
            WriteCentered("");
            //WriteCentered($"ID: {order.OrderId}, User: {order.UserId}, Amount: ${order.TotalAmount}, Status: {order.Status}, Payment: {order.PaymentMethod}");
            WriteCentered($"ID: {order.OrderId}, User: {order.UserId}, Amount: ${order.TotalAmount} ");
            WriteCentered($"Shipping Address: {order.ShippingAddress}\n");
            WriteCentered("Products:");
            foreach (var p in order.ProdList)
            {
                WriteCentered($"Product id: {p.ProductId}, Product: {p.ProductName}, Quantity: {p.Quantity}, Price: ${p.Price}, Status: {p.Status}");
            }
            WriteCentered("");
            WriteCentered($"Payment: {paymentMode2}");

            WriteCentered("");
        }
        return true;

    }
    public bool ViewOrdersForMerchant(Merchant m)
    {
        if (orders.Count == 0)
        {
            Console.Clear();
            WriteCentered("No orders found.");
            return false;
        }
        WriteCentered("Orders:\n");
        foreach (var order in orders)
        {
            string paymentMode2 = "";
            if (order.PaymentMethod == PaymentMode.UPI)
            {
                paymentMode2 = order.UPIMethod.ToString();
            }
            else if (order.PaymentMethod == PaymentMode.Card)
            {
                paymentMode2 = order.PaymentMethod.ToString();
            }
            WriteCentered("");
            //WriteCentered($"ID: {order.OrderId}, User: {order.UserId}, Amount: ${order.TotalAmount}, Status: {order.Status}, Payment: {order.PaymentMethod}");
            WriteCentered($"ID: {order.OrderId}, User: {order.UserId}, Amount: ${order.TotalAmount} ");
            WriteCentered($"Shipping Address: {order.ShippingAddress}\n");
            WriteCentered("Products:");
            foreach (var p in order.ProdList)
            {
                if(p.MerchName == m.Username)
                {
                    WriteCentered($"Product id: {p.ProductId}, Product: {p.ProductName}, Quantity: {p.Quantity}, Price: ${p.Price}, Status: {order.Status}");
                }
                //WriteCentered($"Product id: {p.ProductId}, Product: {p.ProductName}, Quantity: {p.Quantity}, Price: ${p.Price}, Status: {order.Status}");
            }
            WriteCentered("");
            WriteCentered($"Payment: {paymentMode2}");

            WriteCentered("");
        }
        return true;

    }

    public void UpdateOrderStatus(int orderId,int pid)
    {
        var order = orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
        {
            Console.Clear();
            WriteCentered("Order not found!");
            return;
        }

        CartItem product = order.ProdList.FirstOrDefault(p => p.ProductId == pid);

        if (product.Status == OrderStatus.Delivered)
        {
            WriteCentered("Order already delivered!");
            return;
        }
        product.Status = OrderStatus.Delivered;
        WriteCentered($"Product {product.ProductId} - {product.ProductName} in Order #{orderId} marked as Delivered!");
    }

   
    public void RecieveOrder(int orderId,int pid)
    {
        var order = orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
        {
            Console.Clear();
            WriteCentered("Order not found!");
            return;
        }
        CartItem product = order.ProdList.FirstOrDefault(p => p.ProductId == pid);

        if (product.Status == OrderStatus.NotDelivered)
        {
            WriteCentered("Order already marked as Not Delivered!");
            return;
        }
        if (product.Status == OrderStatus.Delivered)
        {
            WriteCentered("Order already delivered!");
            return;
        }
        product.Status = OrderStatus.NotDelivered;
        WriteCentered($"Product {product.ProductId} - {product.ProductName} in Order #{orderId} recieved !");
    }
}