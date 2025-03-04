using System;
using System.Collections.Generic;
using System.Linq;
using ECommerceAppFinal.Exceptions;

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
        try
        {
            if (quantity > product.Stock)
            {
                throw new NotEnoughStockException($"Not enough stock for {product.Name}!");
            }

            if (!carts.ContainsKey(userId))
                carts[userId] = new List<CartItem>();

            var existingItem = carts[userId].FirstOrDefault(c => c.ProductId == product.ProductId);
            if (existingItem != null)
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            else
                carts[userId].Add(new CartItem(product.ProductId, product.Name, product.Price, quantity));

            product.Stock -= quantity; // Update product stock

            Console.Clear();
            WriteCentered($"{quantity} x {product.Name} => Added to cart!");
        }
        catch (NotEnoughStockException e)
        {
            WriteCentered(e.Message);
        }
        catch (Exception e)
        {
            WriteCentered($"Unexpected error: {e.Message}");
        }
    }

    public void RemoveFromCart(int userId, int productId)
    {
        try
        {
            //ViewCart(userId,);
            if (!carts.TryGetValue(userId, out List<CartItem>? value) || value.Count == 0)
            {
                throw new CartEmptyException("Your cart is empty!");
            }
            if (!carts.TryGetValue(userId, out List<CartItem>? value1) || !value1.Any(c => c.ProductId == productId))
            {
                throw new ProductNotFoundException("Product not found in cart!");
            }

            var itemToRemove = value.FirstOrDefault(c => c.ProductId == productId);
            if (itemToRemove != null)
            {
                Product product = ProductService.Products.Find(p => p.ProductId == productId);
                product.Stock += itemToRemove.Quantity; // Restore product stock

                value.Remove(itemToRemove);
                WriteCentered("Product removed from cart!");
            }
        }
        catch (ProductNotFoundException e)
        {
            WriteCentered(e.Message);
        }
        catch (Exception e)
        {
            WriteCentered($"Unexpected error: {e.Message}");
        }
    }

    public void ViewCart(int userId, OrderService o)
    {
        try
        {
            if (!carts.TryGetValue(userId, out List<CartItem>? value) || value.Count == 0)
            {
                throw new CartEmptyException("Your cart is empty!");
            }
            Console.Clear();
            WriteCentered("Your Cart:");
            foreach (var item in value)
                WriteCentered($"- {item.ProductName} (${item.Price} x {item.Quantity})");

            WriteCentered("Press 1 to Checkout, or any other key to go back.");
            if (ReadCentered("") == "1")
                Checkout(userId, o);
        }
        catch (CartEmptyException e)
        {
            WriteCentered(e.Message);
        }
        catch (Exception e)
        {
            WriteCentered($"Unexpected error: {e.Message}");
        }
    }

    public void Checkout(int userId, OrderService o)
    {
        try
        {
            if (!carts.TryGetValue(userId, out List<CartItem>? value) || value.Count == 0)
            {
                throw new CartEmptyException("Your cart is empty! Cannot checkout.");
            }

            double totalAmount = value.Sum(c => c.Price * c.Quantity);
            WriteCentered($"Total Amount: ${totalAmount}");
            //WriteCentered("Select Payment Mode: 1. Card 2. UPI");
            //PaymentMode paymentMode = (Console.ReadLine() == "1") ? PaymentMode.Card : PaymentMode.UPI;
            string choice = ReadCentered("Select Payment Mode: 1. Card 2. UPI");
            if (string.IsNullOrEmpty(choice))
            {
                throw new InvalidChoiceException("Enter valid choice.");
            }
            if(choice == "1")
            {
                PaymentMode paymentMode = PaymentMode.Card;
                o.PlaceOrder(userId, totalAmount, paymentMode);
            }
            else if (choice == "2")
            {
                PaymentMode paymentMode = PaymentMode.UPI;
                o.PlaceOrder(userId, totalAmount, paymentMode);
            }
            else
            {
                throw new InvalidChoiceException("Enter valid choice.");
            }

            //o.PlaceOrder(userId, totalAmount, paymentMode);

            foreach (var item in value)
            {
                Product product = ProductService.Products.Find(p => p.ProductId == item.ProductId);
                product.Stock -= item.Quantity; // Update product stock
            }
            carts.Remove(userId); // Empty cart after checkout
        }
        catch (CartEmptyException e)
        {
            WriteCentered(e.Message);
        }
        catch (Exception e)
        {
            WriteCentered($"Unexpected error: {e.Message}");
        }
    }
}
