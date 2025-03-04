using ECommerceAppFinal.Exceptions;

class ProductService : IProductService
{
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


    //public string ProductFilePath = "C:\\Users\\10decoders\\source\\repos\\ECommerceAppFinal\\ECommerceAppFinal\\Data\\product.txt";
    public static List<Product> Products = new List<Product>();

    public static double CalcAvgRating(string productname)
    {
        Product product = Products.Find(p => p.Name == productname);
        if (product == null || product.Reviews.Count == 0)
        {
            return 0; // No reviews, return 0
        }

        double sum = 0;
        foreach (var r in product.Reviews)
        {
            if (r.productname == productname)
                sum += (int)r.reviewtype;
        }
        return sum / product.Reviews.Count;
    }

    public void AddProduct(Merchant m, string name, double price, int stock, Category categories)
    {
        try
        {
            int productId = Products.Count + 1;
            Product newProduct = new Product(productId, name, price, stock, categories, m.Username);
            Products.Add(newProduct);
            m.products.Add(newProduct);

            Console.Clear();
            WriteCentered("Product added successfully!");
        }
        catch (Exception ex)
        {
            WriteCentered($"Error : {ex.Message}");
        }
        finally
        {
            WriteCentered("Add product executed");
        }
    }

    public List<Product> GetAllProducts() { return Products; }
    public List<Product> GetProductsByCategory(Category category)
    {
        return Products.Where(p => p.Categories.Equals(category)).ToList();
    }

    public List<Product> SearchProduct(string keyword)
    {
        return Products.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Product> SortProductsByPrice(bool ascending)
    {
        return ascending ? Products.OrderBy(p => p.Price).ToList() : Products.OrderByDescending(p => p.Price).ToList();
    }

    public bool DisplayProducts(List<Product> products)
    {
        if (products.Count < 1)
        {
            Console.Clear();
            WriteCentered("No products found.\n");
            return false;
        }

        int maxNameLength = products.Max(p => p.Name.Length);
        int maxIdLength = products.Max(p => p.ProductId.ToString().Length);
        int maxPriceLength = products.Max(p => p.Price.ToString().Length);
        int maxStockLength = products.Max(p => p.Stock.ToString().Length);
        int maxRatingLength = products.Max(p => CalcAvgRating(p.Name).ToString().Length);
        int maxMerchantLength = products.Max(p => p.MerchName.Length);

        string format = $"{{0, -{maxNameLength}}}\t{{1, -{maxNameLength}}}\t{{2, -{maxNameLength}}}";

        for (int i = 0; i < products.Count; i += 3)
        {
            if (i + 2 < products.Count)
            {
                Console.WriteLine(format, $"Name: {products[i].Name}", $"Name: {products[i + 1].Name}", $"Name: {products[i + 2].Name}");
                Console.WriteLine(format, $"Product ID: {products[i].ProductId}", $"Product ID: {products[i + 1].ProductId}", $"Product ID: {products[i + 2].ProductId}");
                Console.WriteLine(format, $"Price: {products[i].Price}", $"Price: {products[i + 1].Price}", $"Price: {products[i + 2].Price}");
                Console.WriteLine(format, $"Available: {products[i].Stock}", $"Available: {products[i + 1].Stock}", $"Available: {products[i + 2].Stock}");
                Console.WriteLine(format, $"Rating: {CalcAvgRating(products[i].Name)}", $"Rating: {CalcAvgRating(products[i + 1].Name)}", $"Rating: {CalcAvgRating(products[i + 2].Name)}");
                Console.WriteLine(format, $"Merchant: {products[i].MerchName}", $"Merchant: {products[i + 1].MerchName}", $"Merchant: {products[i + 2].MerchName}");
            }
            else if (i + 1 < products.Count)
            {
                Console.WriteLine(format, $"Name: {products[i].Name}", $"Name: {products[i + 1].Name}", "");
                Console.WriteLine(format, $"Product ID: {products[i].ProductId}", $"Product ID: {products[i + 1].ProductId}", "");
                Console.WriteLine(format, $"Price: {products[i].Price}", $"Price: {products[i + 1].Price}", "");
                Console.WriteLine(format, $"Available: {products[i].Stock}", $"Available: {products[i + 1].Stock}", "");
                Console.WriteLine(format, $"Rating: {CalcAvgRating(products[i].Name)}", $"Rating: {CalcAvgRating(products[i + 1].Name)}", "");
                Console.WriteLine(format, $"Merchant: {products[i].MerchName}", $"Merchant: {products[i + 1].MerchName}", "");
            }
            else
            {
                Console.WriteLine(format, $"Name: {products[i].Name}", "", "");
                Console.WriteLine(format, $"Product ID: {products[i].ProductId}", "", "");
                Console.WriteLine(format, $"Price: {products[i].Price}", "", "");
                Console.WriteLine(format, $"Available: {products[i].Stock}", "", "");
                Console.WriteLine(format, $"Rating: {CalcAvgRating(products[i].Name)}", "", "");
                Console.WriteLine(format, $"Merchant: {products[i].MerchName}", "", "");
            }
            Console.WriteLine();
        }
        return true;
    }

    public bool RemoveProductById(int id, Merchant m)
    {
        try
        {
            Product productToRemove = Products.FirstOrDefault(p => p.ProductId == id);
            Product prd = m.products.FirstOrDefault(p => p.ProductId == id);
            if (productToRemove == null || prd == null)
            {
                throw new ProductNotFoundException($"Product with ID {id} not found.");
            }

            Products.Remove(productToRemove);
            m.products.Remove(prd);
            Console.Clear();

            WriteCentered($"Product {id} removed successfully.");
            return true;
        }
        catch (ProductNotFoundException ex)
        {
            WriteCentered(ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            WriteCentered($"Unexpected error: {ex.Message}");
            return false;
        }
    }
    public bool SearchAndBuy(Customer user,CartService cart)
    {
        int oid;
        string confirm;
        x:
        string keyword = ReadCentered("Enter keyword : ");
        try
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new InvalidProductDetailsException("Enter valid product name.");
            }
        }
        catch (InvalidProductDetailsException ex)
        {
            WriteCentered(ex.Message);
            return false;
        }
        List<Product> foundprods = SearchProduct(keyword);
        try
        {
            if (foundprods.Count == 0)
            {
                throw new ProductNotFoundException("No products found.");
            }
            else
            {
                DisplayProducts(foundprods);

                confirm = ReadCentered("Do you want to buy the product?(y/n):");
                if(confirm == "y")
                {
                    y:
                    oid = int.Parse(ReadCentered("Enter product id to add to cart : "));
                    try
                    {
                        if (oid < 1)
                        {
                            throw new InvalidProductDetailsException("Invalid product id! Try again.");
                        }
                        else
                        {
                            foreach (Product product in foundprods)
                            {
                                if (product.ProductId == oid)
                                {
                                    WriteCentered("Enter quantity : ");
                                    int q = int.Parse(ReadCentered(""));
                                    cart.AddToCart(user.UserId, product, q);
                                }
                            }
                        }
                    }
                    catch (InvalidProductDetailsException ex)
                    {
                        WriteCentered(ex.Message);
                        goto y;
                    }
                }
                else
                {
                    confirm = ReadCentered("Do you want to search another product? (y/n) :");
                    if (confirm != "y")
                    {
                        //.Clear();
                        return false;
                    }
                    else
                    {
                        goto x;
                    }
                }
                
            }
        }
        catch (ProductNotFoundException ex)
        {
            WriteCentered(ex.Message);
            confirm = ReadCentered("Do you want to search another product? (y/n) :");
            if (confirm != "y") 
            {
                return false;
            }
            else
            {
                goto x;
            }
            
        }
        return true;
    }
}
