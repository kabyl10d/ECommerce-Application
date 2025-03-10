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
            WriteCentered("No products found in the inventory.\n");
            return false;
        }

        int maxNameLength = products.Max(p => p.Name.Length);
        int maxIdLength = products.Max(p => p.ProductId.ToString().Length);
        int maxPriceLength = products.Max(p => p.Price.ToString().Length);
        int maxStockLength = products.Max(p => p.Stock.ToString().Length);
        int maxRatingLength = products.Max(p => CalcAvgRating(p.Name).ToString().Length);
        int maxMerchantLength = products.Max(p => p.MerchName.Length);

        maxNameLength = 50;
        //int max = Math.Max(maxNameLength, Math.Max(maxIdLength, Math.Max(maxPriceLength, Math.Max(maxStockLength, Math.Max(maxRatingLength, maxMerchantLength)))));
        //string format = $"{{0, -{maxNameLength}}}\t{{1, -{maxNameLength}}}\t{{2, -{maxNameLength}}}";
        string format = $"{{0, -{maxNameLength}}}\t{{1, -{maxNameLength}}}\t{{2, -{maxNameLength}}}";

        for (int i = 0; i < products.Count; i += 3)
        {
            if (i + 2 < products.Count)
            {
                Console.WriteLine(format, $"Name: {products[i].Name}", $"Name: {products[i + 1].Name}", $"Name: {products[i + 2].Name}");
                Console.WriteLine(format, $"Product ID: {products[i].ProductId}", $"Product ID: {products[i + 1].ProductId}", $"Product ID: {products[i + 2].ProductId}");
                Console.WriteLine(format, $"Price: {products[i].Price}", $"Price: {products[i + 1].Price}", $"Price: {products[i + 2].Price}");
                Console.WriteLine(format, $"Available: {products[i].Stock}", $"Available: {products[i + 1].Stock}", $"Available: {products[i + 2].Stock}");
                Console.WriteLine(format, $"Rating: {CalcAvgRating(products[i].Name)} ({products[i].Reviews.Count} votes)", $"Rating: {CalcAvgRating(products[i + 1].Name)} ({products[i + 1].Reviews.Count} votes)", $"Rating: {CalcAvgRating(products[i + 2].Name)} ({products[i + 2].Reviews.Count} votes)");
                Console.WriteLine(format, $"Merchant: {products[i].MerchName}", $"Merchant: {products[i + 1].MerchName}", $"Merchant: {products[i + 2].MerchName}");
            }
            else if (i + 1 < products.Count)
            {
                Console.WriteLine(format, $"Name: {products[i].Name}", $"Name: {products[i + 1].Name}", "");
                Console.WriteLine(format, $"Product ID: {products[i].ProductId}", $"Product ID: {products[i + 1].ProductId}", "");
                Console.WriteLine(format, $"Price: {products[i].Price}", $"Price: {products[i + 1].Price}", "");
                Console.WriteLine(format, $"Available: {products[i].Stock}", $"Available: {products[i + 1].Stock}", "");
                Console.WriteLine(format, $"Rating: {CalcAvgRating(products[i].Name)} ({products[i].Reviews.Count} votes)", $"Rating: {CalcAvgRating(products[i + 1].Name)} ({products[i + 1].Reviews.Count} votes)", "");
                Console.WriteLine(format, $"Merchant: {products[i].MerchName}", $"Merchant: {products[i + 1].MerchName}", "");
            }
            else
            {
                Console.WriteLine(format, $"Name: {products[i].Name}", "", "");
                Console.WriteLine(format, $"Product ID: {products[i].ProductId}", "", "");
                Console.WriteLine(format, $"Price: {products[i].Price}", "", "");
                Console.WriteLine(format, $"Available: {products[i].Stock}", "", "");
                Console.WriteLine(format, $"Rating: {CalcAvgRating(products[i].Name)} ({products[i].Reviews.Count} votes)", "", "");
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

    public bool DeleteProduct(string name)
    {
        try
        {
            List<Product> productsToRemove = Products.Where(p => p.Name == name).ToList();
            if (productsToRemove.Count == 0)
            {
                throw new ProductNotFoundException($"Product with name {name} not found.");
            }
            foreach (var p in productsToRemove)
            {
                Products.Remove(p);
            }
            Console.Clear();

            WriteCentered($"Product {name} removed successfully.");
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
    public bool DeleteProduct(int id)
    {
        try
        {
            Product productToRemove = Products.FirstOrDefault(p => p.ProductId == id);
            //Product prd = m.products.FirstOrDefault(p => p.ProductId == id);
            if (productToRemove == null)
            {
                throw new ProductNotFoundException($"Product with ID {id} not found.");
            }

            Products.Remove(productToRemove);
            //m.products.Remove(prd);
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
    public bool SearchAndBuy(Customer user, CartService cart, ReviewService reviewService)
    {
        string oid;
        string confirm, confirm2;
    x:
        string keyword = ReadCentered("Enter keyword : ");

        if (keyword == "0")
        {
            return false;
        }

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
            goto x;
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
            c3:

                DisplayProducts(foundprods);
                WriteCentered("1. Add a product to cart");
                WriteCentered("2. Review a product");
                WriteCentered("3. View Reviews of a product");
                WriteCentered("Press 0 to back");
                confirm2 = ReadCentered("Enter choice : ");
                try
                {
                    if (string.IsNullOrEmpty(confirm2))
                    {
                        throw new InvalidProductDetailsException("Enter valid option.");
                    }

                }
                catch (InvalidProductDetailsException ex)
                {
                    WriteCentered(ex.Message);
                    goto c3;
                }
                if (confirm2 == "0")
                {
                    Console.Clear();
                    return false;
                }
                if (confirm2 == "1")
                {
                ym:
                    confirm = ReadCentered("Do you want to cart a product?(y/n):");
                    if (confirm == "y")
                    {
                    y:
                        oid = ReadCentered("Enter product id to add to cart (0 to cancel): ");
                        try
                        {

                            if (oid == "0")
                            {
                                goto c3;
                            }
                            if (string.IsNullOrEmpty(oid))
                            {
                                throw new InvalidProductDetailsException("Enter valid product id.");
                            }
                            if (int.Parse(oid) < 1)
                            {
                                throw new InvalidProductDetailsException("Invalid product id! Try again.");
                            }


                            foreach (Product product in foundprods)
                            {
                                if (product.ProductId == int.Parse(oid))
                                {
                                quant:
                                    WriteCentered("");
                                    string q = ReadCentered("Enter quantity : ");
                                    if (q == "0")
                                    {
                                        goto c3;
                                    }
                                    if (string.IsNullOrEmpty(q) || int.Parse(q) < 1)
                                    {
                                        WriteCentered("Enter valid quantity.");
                                        goto quant;
                                    }
                                    if (!cart.AddToCart(user.UserId, product, int.Parse(q)))   //continue with adding goto labels
                                    {
                                        goto quant;
                                    }
                                    else
                                    {
                                    y1:
                                        WriteCentered("");
                                        confirm = ReadCentered("Do you want to search another product? (y/n) :");
                                        if (confirm == "n")
                                        {
                                            //.Clear();
                                            goto c3;
                                            //return false;
                                        }
                                        else if (confirm == "y")
                                        {
                                            goto x;
                                        }
                                        else
                                        {
                                            WriteCentered("Invalid choice! Try again.");
                                            goto y1;
                                        }

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
                    else if (confirm == "n")
                    {
                    //search:
                    y2:
                        confirm = ReadCentered("Do you want to search another product? (y/n) :");
                        if (confirm == "n")
                        {
                            //goto c3;
                            //.Clear();
                            return false;
                        }
                        else if (confirm == "y")
                        {
                            goto x;
                        }
                        else
                        {
                            WriteCentered("Invalid choice! Try again.");
                            goto y2;
                        }
                    }
                    else
                    {
                        WriteCentered("Invalid choice! Try again.");
                        goto ym;
                    }

                }
                else if (confirm2 == "2")
                {
                    int prid = int.Parse(ReadCentered("Enter product id to review (0 to back) : "));
                    if (prid == 0)
                    {
                        Console.Clear();
                        goto c3;
                    }
                    Product p = GetAllProducts().Find(p => p.ProductId == prid);
                    if (p != null)
                    {
                    rt:
                        WriteCentered("Select review type :");
                        WriteCentered("1. Critical");
                        WriteCentered("2. NotBad");
                        WriteCentered("3. Good");
                        WriteCentered("4. VeryGood");
                        WriteCentered("5. Excellent");
                        int rt = int.Parse(ReadCentered("Enter choice : "));
                        try
                        {
                            if (rt == 0)
                            {
                                Console.Clear();
                                goto c3;
                            }
                            if (rt < 1 || rt > 5)
                            {
                                throw new InvalidChoiceException("Invalid review choice! Try again.");
                            }
                        }
                        catch (InvalidChoiceException ex)
                        {
                            WriteCentered(ex.Message);
                            goto rt;
                        }
                        //WriteCentered("Enter review text : ");
                        string reviewtext = ReadCentered("Enter review text : ");
                        if (reviewtext == "0")
                        {
                            Console.Clear();
                            goto c3;
                        }
                        reviewService.AddReview((Customer)user, p, (ReviewType)rt, reviewtext);
                        goto c3;
                    }
                }
                else if (confirm2 == "3")
                {
                    int prid = int.Parse(ReadCentered("Enter product id to view reviews (0 to back) : "));
                    if (prid == 0)
                    {
                        Console.Clear();
                        goto c3;
                    }
                    Product p = GetAllProducts().Find(p => p.ProductId == prid);
                    if (p != null)
                    {
                        reviewService.ShowReview(p);
                        goto c3;
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


    //public int GetProductCountByCategory(Category cat)
    //{
    //    if (ProductService.Products.Count == 0)
    //    {
    //        //WriteCentered("No products found in the inventory.");
    //        return 0;
    //    }
    //    else
    //    {
    //        int count = ProductService.Products.Count(p => p.Categories == cat);
    //        return count;
    //    }
    //}

    public int GetProductCountByCategory(Category cat)
    {
        if (ProductService.Products.Count == 0)
        {
            return 0;
        }
        else
        {
            int count = ProductService.Products.Where(p => p.Categories == cat).Select(p => p.Name).Distinct().Count();
            return count;
        }
    }


    public int GetTotalStockCount(string productname)
    {
        List<Product> products = Products.Where(p => p.Name == productname).ToList();
        if (products.Count == 0)
        {
            return 0;
        }
        return products.Sum(p => p.Stock);
    }

    public double GetTotalPrice(string productname)
    {
        List<Product> products = Products.Where(p => p.Name == productname).ToList();
        double total = 0;
        foreach (Product p in products)
        {
            total += p.Price * p.Stock;
        }
        return total;

    }

    public void DisplayProductNamesWithCount(Category category)
    {
        //var productNames = Products.Select(p => p.Name).Distinct();
        var productNames = Products.Where(p => p.Categories == category).Select(p => p.Name).Distinct();
        WriteCentered("");
        var productids = Products.Where(p => p.Categories == category).Select(p => p.ProductId).Distinct().ToList();
        foreach (string name in productNames)
        {
            int count = GetTotalStockCount(name);
            WriteCentered($"Name :  {name} , Total stock :  {count}");
        }
    }

    public bool DisplayProductsWithDetails(string name, Category category)
    {
        List<Product> products = Products.Where(p => p.Name == name).ToList();
        var productNames = Products.Where(p => p.Categories == category && p.Name == name).Select(p => p.Name).Distinct();

        if (products.Count == 0)
        {
            WriteCentered("No products found in the inventory.");
            return false;
        }
        else
        {
            WriteCentered(" ");
            WriteCentered($"{name}");
            WriteCentered($"Total amount Rs. {GetTotalPrice(name)}");
            foreach (var pname in productNames)
            {
                List<Product> p = SearchProduct(pname);
                foreach (Product pr in p)
                {
                    WriteCentered($"Merchant: {pr.MerchName}, Product ID: {pr.ProductId}, Price: Rs. {pr.Price}, Stock: {pr.Stock}");
                }
            }
            return true;
        }
    }

    public bool UpdateProductStock(int id)
    {
        string newStock = ReadCentered("Enter new stock: ");
        try
        {
            if (string.IsNullOrEmpty(newStock))
            {
                throw new InvalidProductDetailsException("Enter valid stock.");
            }
            if (newStock == "0")
            {
                return false;
            }
            Product product = Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return false;
            }
            product.Stock = int.Parse(newStock);
            Console.Clear();
            WriteCentered($"Stock for {product.Name} updated successfully.");
            return true;
        }
        catch (ProductNotFoundException ex)
        {
            WriteCentered(ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            WriteCentered($"{ex.Message}");
            return false;
        }
    }

}

