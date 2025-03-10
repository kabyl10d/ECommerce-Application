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
    public bool AddToCart(int userId, Product product, int quantity)
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
                carts[userId].Add(new CartItem(product.ProductId, product.Name, product.Price, quantity, product.MerchName));

            product.Stock -= quantity; // Update product stock

            Console.Clear();
            WriteCentered($"{quantity} x {product.Name} => Added to cart!");
            return true;
        }
        catch (NotEnoughStockException e)
        {
            WriteCentered(e.Message);
            return false;
        }
        catch (Exception e)
        {
            WriteCentered($"Unexpected error: {e.Message}");
            return false;
        }
    }

    //public void UpdateCart(int userId, int productId, int newCount)
    //{
    //    try
    //    {
    //        if (newCount <= 0)
    //        {
    //            throw new InvalidProductDetailsException("Quantity must be greater than zero.");
    //        }

    //        if (!carts.TryGetValue(userId, out List<CartItem>? value) || value.Count == 0)
    //        {
    //            throw new CartEmptyException("Your cart is empty!");
    //        }

    //        var itemToUpdate = value.FirstOrDefault(c => c.ProductId == productId);
    //        if (itemToUpdate == null)
    //        {
    //            throw new ProductNotFoundException("Product not found in cart!");
    //        }

    //        Product product = ProductService.Products.Find(p => p.ProductId == productId);
    //        if (product == null)
    //        {
    //            throw new ProductNotFoundException("Product not found in inventory!");
    //        }

    //        int currentQuantity = itemToUpdate.Quantity;
    //        int stockDifference = newCount - currentQuantity;

    //        if (stockDifference > product.Stock)
    //        {
    //            throw new NotEnoughStockException($"Not enough stock for {product.Name}!");
    //        }

    //        itemToUpdate.UpdateQuantity(newCount);
    //        product.Stock -= stockDifference; // Update product stock

    //        WriteCentered($"Updated {product.Name} quantity to {newCount} in cart.");
    //    }
    //    catch (InvalidProductDetailsException e)
    //    {
    //        WriteCentered(e.Message);
    //    }
    //    catch (CartEmptyException e)
    //    {
    //        WriteCentered(e.Message);
    //    }
    //    catch (ProductNotFoundException e)
    //    {
    //        WriteCentered(e.Message);
    //    }
    //    catch (NotEnoughStockException e)
    //    {
    //        WriteCentered(e.Message);
    //    }
    //    catch (Exception e)
    //    {
    //        WriteCentered($"Unexpected error: {e.Message}");
    //    }
    //}



    public void UpdateCart(int userId, int productId, int newCount)
    {
        try
        {
            if (newCount <= 0)
            {
                throw new InvalidProductDetailsException("Quantity must be greater than zero.");
            }

            if (!carts.TryGetValue(userId, out List<CartItem>? value) || value.Count == 0)
            {
                throw new CartEmptyException("Your cart is empty!");
            }

            var itemToUpdate = value.FirstOrDefault(c => c.ProductId == productId);
            if (itemToUpdate == null)
            {
                throw new ProductNotFoundException("Product not found in cart!");
            }

            Product product = ProductService.Products.Find(p => p.ProductId == productId);
            if (product == null)
            {
                throw new ProductNotFoundException("Product not found in inventory!");
            }

            int currentQuantity = itemToUpdate.Quantity;
            int stockDifference = newCount - currentQuantity;

            if (stockDifference > 0 && stockDifference > product.Stock)
            {
                throw new NotEnoughStockException($"Not enough stock for {product.Name}!");
            }

            itemToUpdate.UpdateQuantity(newCount);
            product.Stock -= stockDifference;

            if (stockDifference < 0)
            {
                product.Stock += Math.Abs(stockDifference);
            }

            WriteCentered($"Updated {product.Name} quantity to {newCount} in cart.");
        }
        catch (InvalidProductDetailsException e)
        {
            WriteCentered(e.Message);
        }
        catch (CartEmptyException e)
        {
            WriteCentered(e.Message);
        }
        catch (ProductNotFoundException e)
        {
            WriteCentered(e.Message);
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

    public bool DisplayCartItems(int userId)
    {
        try
        {
            if (!carts.TryGetValue(userId, out List<CartItem>? value) || value.Count == 0)
            {
                throw new CartEmptyException("Your cart is empty!");
            }
            WriteCentered("");
            WriteCentered("Your Cart:");
            WriteCentered("");

            foreach (var item in value)
                WriteCentered($"Id {item.ProductId}. {item.ProductName} => (${item.Price} x {item.Quantity})");
            return true;
        }
        catch (CartEmptyException e)
        {
            WriteCentered(e.Message);
            return false;
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
            WriteCentered("");
            WriteCentered("Your Cart:");
            WriteCentered("");

            foreach (var item in value)
                WriteCentered($"Product id: {item.ProductId}, Name: {item.ProductName} => (Rs.{item.Price} x {item.Quantity})");

            WriteCentered("Press 1 to Checkout, or any other key to go back.");
            if (ReadCentered("") == "1")
                Checkout(userId, o, value);
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

    public void Checkout(int userId, OrderService o, List<CartItem> prodlist)
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
            WriteCentered("Select Payment Mode: 1. Card 2. UPI  (0 to cancel payment)");
            string choice = ReadCentered("Enter choice :");
            if (choice == "0")
            {
                throw new PaymentFailedException("Payment cancelled!");
            }
            if (string.IsNullOrEmpty(choice))
            {
                throw new InvalidChoiceException("Enter valid choice.");
            }
            if (choice == "1")
            {
                PaymentMode paymentMode = PaymentMode.Card;
                if (o.PlaceOrder(userId, prodlist, totalAmount, paymentMode))
                {

                    carts.Remove(userId);
                    //foreach (var item in value)
                    //{
                    //    Product product = ProductService.Products.Find(p => p.ProductId == item.ProductId);
                    //    product.Stock -= item.Quantity; // Update product stock
                    //}
                }
            }
            else if (choice == "2")
            {
                PaymentMode paymentMode = PaymentMode.UPI;
                if (o.PlaceOrder(userId, prodlist, totalAmount, paymentMode))
                {
                    carts.Remove(userId);
                    //foreach (var item in value)
                    //{
                    //    Product product = ProductService.Products.Find(p => p.ProductId == item.ProductId);
                    //    product.Stock -= item.Quantity; // Update product stock
                    //}
                }
            }
            else
            {
                throw new InvalidChoiceException("Enter valid choice.");
            }

            //o.PlaceOrder(userId, totalAmount, paymentMode);

            //foreach (var item in value)
            //{
            //    Product product = ProductService.Products.Find(p => p.ProductId == item.ProductId);
            //    product.Stock -= item.Quantity; // Update product stock
            //}
            // Empty cart after checkout
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
