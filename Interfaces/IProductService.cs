public interface IProductService
{
    void AddProduct(Merchant m, string name, double price, int stock, Category categories);
    List<Product> GetAllProducts();
    List<Product> GetProductsByCategory(Category category);
    List<Product> SearchProduct(string keyword);
    List<Product> SortProductsByPrice(bool ascending);

    void DisplayProducts(List<Product> products);

    bool RemoveProductById(int productid,Merchant m);
}
