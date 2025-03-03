class ProductService : IProductService
{
    private static void WriteCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.WriteLine(new string(' ', spaces) + text);
    }
     
    //public string ProductFilePath = "C:\\Users\\10decoders\\source\\repos\\ECommerceAppFinal\\ECommerceAppFinal\\Data\\product.txt";
    public static List<Product> Products = new List<Product>();

    public double CalcAvgRating(string productname)
    {
        Product product = Products.Find(p => p.Name == productname);
        double sum = 0;
        foreach (var r in product.Reviews)
        {
            if (r.productname == productname)
                sum += (int)r.reviewtype;
        }
        return sum / product.Reviews.Count;

    }

    public void AddProduct(Merchant m,string name, double price, int stock, Category categories)
    {
        try
        {
            int productId = Products.Count + 1;
            Products.Add(new Product(productId, name, price, stock, categories,m.Username));
            m.products.Add(new Product(productId, name, price, stock, categories, m.Username));

            WriteCentered("Product added successfully!");
        }
        catch(Exception ex) 
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
        return ascending? Products.OrderBy(p => p.Price).ToList() : Products.OrderByDescending(p => p.Price).ToList();
    }

    

    public void DisplayProducts(List<Product> Products)
    {
        if(Products.Count < 1) { WriteCentered("No products found.\n"); return; }
        int maxNameLength = Products.Max(p => p.Name.Length);
        string format = $"{{0, -{maxNameLength}}}\t{{1, -{maxNameLength}}}\t{{2, -{maxNameLength}}}";

        for (int i = 0; i < Products.Count; i += 3)
        {
            if (i + 2 < Products.Count)
            {
                Console.WriteLine(format, $"Name: {Products[i].Name}",$"Name: {Products[i + 1].Name}",$"Name: {Products[i + 2].Name}");
                Console.WriteLine(format, $"Product ID: {Products[i].ProductId}", $"Product ID: {Products[i + 1].ProductId}", $"Product ID: {Products[i + 2].ProductId}");
                Console.WriteLine(format, $"Price: {Products[i].Price}", $"Price: {Products[i + 1].Price}", $"Price: {Products[i + 2].Price}");
                Console.WriteLine(format, $"Available: {Products[i].Stock}", $"Available: {Products[i + 1].Stock}", $"Available: {Products[i + 2].Stock}");
                Console.WriteLine(format, $"Rating: {CalcAvgRating(Products[i].Name)}", $"Rating: {CalcAvgRating(Products[i+1].Name)}", $"Rating: {CalcAvgRating(Products[i+2].Name)}");
                Console.WriteLine(format, $"Merchant: {Products[i].MerchName}", $"Merchant: {Products[i + 1].MerchName}", $"Merchant: {Products[i+2].MerchName}");
            }
            else if (i + 1 < Products.Count)
            {
                Console.WriteLine(format, $"Name: {Products[i].Name}", $"Name: {Products[i + 1].Name}", "");
                Console.WriteLine(format, $"Product ID: {Products[i].ProductId}", $"Product ID: {Products[i + 1].ProductId}", "");
                Console.WriteLine(format, $"Price: {Products[i].Price}", $"Price: {Products[i + 1].Price}", "");
                Console.WriteLine(format, $"Available: {Products[i].Stock}", $"Available: {Products[i + 1].Stock}", "");
                Console.WriteLine(format, $"Rating: {CalcAvgRating(Products[i].Name)}", $"Rating: {CalcAvgRating(Products[i+1].Name)}", "");
                Console.WriteLine(format, $"Merchant: {Products[i].MerchName}", $"Merchant: {Products[i].MerchName}","");
            }
            else
            {
                Console.WriteLine(format, $"Name: {Products[i].Name}", "", "");
                Console.WriteLine(format, $"Product ID: {Products[i].ProductId}", "", "");
                Console.WriteLine(format, $"Price: {Products[i].Price}", "", "");
                Console.WriteLine(format, $"Available: {Products[i].Stock}", "", "");
                Console.WriteLine(format, $"Rating: {CalcAvgRating(Products[i].Name)}", "", "");
                Console.WriteLine(format, $"Merchant: {Products[i].MerchName}", "", "");
            }
            Console.WriteLine();  
        }
    }

    public bool RemoveProductById(int id,Merchant m)
    {
        Product productToRemove = Products.FirstOrDefault(p => p.ProductId == id);
        Product prd = m.products.FirstOrDefault(p => p.ProductId == id);
        if (productToRemove != null && prd != null)
        {
            Products.Remove(productToRemove);
            m.products.Remove(prd);
            WriteCentered($"Product {id} removed successfully.");
            return true;
        }
        else
        {
            return false;
        }
    }

}