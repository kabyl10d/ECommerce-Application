public class Product
{
    public int ProductId { get; private set; }
    public string Name { get; private set; }
    public double Price { get; private set; }
    public int Stock;

    public string MerchName;
    //public string Review { get; private set; }

    //public int Rating { get; private set; } 
    public Category Categories { get; private set; }

    public List<Review> Reviews = new List<Review>();


    


    
    public Product(int productId, string name, double price, int stock, Category categories, string merchName)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        Stock = stock;
        Categories = categories;
        MerchName = merchName;
    }
}
