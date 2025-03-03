using System;
using System.Collections.Generic;
using System.Linq;

public class CartService : ICartService
{
    private Dictionary<int, List<CartItem>> carts = new Dictionary<int, List<CartItem>>();
    private static void WriteCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.WriteLine(new string(' ', spaces) + text);
    }
    private static string ReadCentered(string prompt)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = prompt.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.Write(new string(' ', spaces) + prompt);
        return Console.ReadLine();
    }
    public void AddToCart(int userId, Product product, int quantity)
    {
        if (quantity > product.Stock)
        {
            WriteCentered($"Not enough stock for {product.Name}!");
            return;
        }

        if (!carts.ContainsKey(userId))
            carts[userId] = new List<CartItem>();

        var existingItem = carts[userId].FirstOrDefault(c => c.ProductId == product.ProductId);
        if (existingItem != null)
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        else
            carts[userId].Add(new CartItem(product.ProductId, product.Name, product.Price, quantity));

        //product.Stock -= quantity;

        WriteCentered($"{quantity} x {product.Name} => Added to cart!");
    }

    public void RemoveFromCart(int userId, int productId)
    {
        if (!carts.ContainsKey(userId) || !carts[userId].Any(c => c.ProductId == productId))
        {
            WriteCentered("Product not found in cart!");
            return;
        }

        carts[userId].RemoveAll(c => c.ProductId == productId);
        WriteCentered("Product removed from cart!");
    }

    public void ViewCart(int userId,OrderService o)
    {
        if (!carts.ContainsKey(userId) || carts[userId].Count == 0)
        {
            WriteCentered("Your cart is empty!");
            return;
        }

        WriteCentered("Your Cart:");
        foreach (var item in carts[userId])
            WriteCentered($"- {item.ProductName} (${item.Price} x {item.Quantity})");

        WriteCentered("Press 1 to Checkout, or any other key to go back.");
        if (ReadCentered("") == "1")
            Checkout(userId,o);
    }

    public void Checkout(int userId,OrderService o)
    {
        if (!carts.ContainsKey(userId) || carts[userId].Count == 0)
        {
            WriteCentered("Your cart is empty! Cannot checkout.");
            return;
        }

        double totalAmount = carts[userId].Sum(c => c.Price * c.Quantity);
        WriteCentered($"Total Amount: ${totalAmount}");
        WriteCentered("Select Payment Mode: 1. Card 2. UPI");
        PaymentMode paymentMode = (Console.ReadLine() == "1") ? PaymentMode.Card : PaymentMode.UPI;

        o.PlaceOrder(userId,totalAmount,paymentMode);
        //Console.WriteLine($"Order placed successfully via {paymentMode}!");

        //product.Stock -= quantity;

        foreach (var item in carts[userId])
        {
            Product product = ProductService.Products.Find(p => p.ProductId == item.ProductId);
            product.Stock -= item.Quantity;
        }
        carts.Remove(userId); // Empty cart after checkout
    }
}
