class Program
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
    public static void Main(string[] args)
    {
        UserService userService = new UserService();
        ProductService productService = new ProductService();
        OrderService orderService = new OrderService();
        CartService cartService = new CartService();
        ReviewService reviewService = new ReviewService();

        try
        {
            WriteCentered("*************************************");
            WriteCentered("*****(TBF) ECOMMERCE APPLICATION*****");
            WriteCentered("*************************************");

            while (true)//login register loop
            {
                WriteCentered("1. Register");
                WriteCentered("2. Login");
                WriteCentered("(Press 0 to Exit)");
                int c1 = int.Parse(ReadCentered("Enter Choice :"));

                if (c1 == 1) //login register menu
                {
                    Console.Clear();
                    WriteCentered("REGISTER AS USER\n");
                un:

                    string rusername = ReadCentered("Enter Username : ");

                    if (string.IsNullOrEmpty(rusername))
                    {
                        WriteCentered("Username cannot be empty! Try again.\n");
                        goto un;
                    }
                    if (UserService.users.Exists(u => u.Username == rusername))
                    {
                        WriteCentered("Username already exists!");
                        goto un;
                    }
                mail:

                    string rmail = ReadCentered("Enter Mail Address : ");

                    if (string.IsNullOrEmpty(rmail))
                    {
                        WriteCentered("Mail address cannot be empty! Try again.\n");
                        goto mail;
                    }
                    if (!userService.ValidEmail(rmail))
                    {
                        WriteCentered("Invalid mail address! Try again.\n");
                        goto mail;
                    }
                pass:
                    string rpasswd = ReadCentered("Enter Password : ");
                    if (string.IsNullOrEmpty(rpasswd))
                    {
                        WriteCentered("Password cannot be empty! Try again.\n");
                        goto pass;
                    }
                    if (!rpasswd.Equals(ReadCentered("Confirm password : ")))
                    {
                        WriteCentered("Passwords do not match! Try again.\n");
                        goto pass;
                    }
                ph:

                    string rphone = ReadCentered("Enter mobile number : ");
                    if (string.IsNullOrEmpty(rphone) || !userService.ValidPhoneNumber(rphone))
                    {
                        WriteCentered("Invalid phone number! Try again.\n");
                        goto ph;
                    }

                role:
                    string rrole = ReadCentered("Enter Role (merchant / customer) : ");

                    if (string.IsNullOrEmpty(rrole) || (rrole.ToLower() != "merchant" && rrole.ToLower() != "customer"))
                    {
                        WriteCentered("Invalid role! Try again.\n");
                        goto role;
                    }
                    if (rrole == "customer")
                    {
                        string upiid = ReadCentered("Enter UPI ID : ");
                        int upipin = int.Parse(ReadCentered("Enter UPI PIN : "));
                        userService.Register(rusername, rmail, rpasswd, rphone, rrole, upiid, upipin);

                    }
                    else
                    {
                        userService.Register(rusername, rmail, rpasswd, rphone, rrole, "", 0);
                    }
                }
                else if (c1 == 2)
                {
                retry:
                    string runmail = ReadCentered("Enter Username / Mail Address : ");
                    string rpasswd = ReadCentered("Enter Password : ");

                    User user = userService.Login(runmail, rpasswd);
                    if (user == null) { goto retry; }

                    if (user.Role == "Merchant")
                    {
                        // productService.DisplayProducts(productService.GetAllProducts());

                        while (true)
                        {
                            WriteCentered("Merchant Menu \n");
                            WriteCentered("1. Add Products");
                            WriteCentered("2. Delete Products");
                            WriteCentered("3. View Products added by this merchant");
                            WriteCentered("4. View Profile");
                            WriteCentered("(Press 0 to exit)");
                           
                            int c2 = int.Parse(ReadCentered("Enter choice : "));
                            if (c2 == 0) { break; }
                            switch (c2)//merchant menu
                            {
                                case 1:
                                    //WriteCentered("Add Product Details\n\nEnter product name : ");
                                    WriteCentered("Add Product Details\n");

                                    string pname = ReadCentered("Enter product name : ");
                                    double price = double.Parse(ReadCentered("Enter price : "));
                                    int stock = int.Parse(ReadCentered("Enter available count in stock : "));
                                catn:
                                    //WriteCentered("Select category :\n\t1. Electronics\n\t2. Mobiles\n\t3. " +
                                    //    "HomeKitchen\n\t4. Fashion\n\t5. Beauty\n\t6. Health\n\t7. BabyProducts\n\t8. Stationary" +
                                    //    "\nEnter category number : ");

                                    WriteCentered("Select category :");
                                    WriteCentered("1. Electronics");
                                    WriteCentered("2. Mobiles");
                                    WriteCentered("3. HomeKitchen");
                                    WriteCentered("4. Fashion");
                                    WriteCentered("5. Beauty");
                                    WriteCentered("6. Health");
                                    WriteCentered("7. BabyProducts");
                                    WriteCentered("8. Stationary");


                                    int catn = int.Parse(ReadCentered("Enter category number : "));
                                    if (catn < 1 || catn > 8)
                                    {
                                        WriteCentered("Invalid category number! Try again.");
                                        goto catn;
                                    }
                                    Category cat = (Category)catn;

                                    productService.AddProduct((Merchant)user, pname, price, stock, cat);
                                    WriteCentered("");

                                    break;

                                case 2:
                                    WriteCentered("Delete a Product\n");
                                    //WriteCentered("");
                                    int pid = int.Parse(ReadCentered("Enter product id : "));
                                    Merchant m1 = (Merchant)user;
                                    if (!productService.RemoveProductById(pid, m1))
                                    {
                                        WriteCentered("\nProduct not found. ");
                                    }
                                    break;

                                case 3:
                                    Merchant m2 = (Merchant)user;
                                    WriteCentered($"\nProducts added by {m2.Username}\n");
                                    productService.DisplayProducts(m2.products);
                                    break;

                                case 4:
                                    user.DisplayDetails();
                                    break;
                            }
                        }
                    }
                    else if (user.Role == "Customer")
                    {
                        WriteCentered("\n*** Products Inventory ***\n");
                        productService.DisplayProducts(productService.GetAllProducts());

                        while (true)
                        {
                            //WriteCentered("\nCustomer Menu\n\n1. Shopping\n2. Cart and Checkout\n3. View Profile\n" +
                            //    "(Press 0 to Exit)\n\nEnter Choice : ");

                            WriteCentered("Customer Menu\n");
                            WriteCentered("1. Shopping");
                            WriteCentered("2. Cart and Checkout");
                            WriteCentered("3. View Profile");
                            WriteCentered("(Press 0 to Exit)\n");
                            int c2 = int.Parse(ReadCentered("Enter Choice : "));
                            if (c2 == 0) { break; }
                            switch (c2)
                            {
                                case 1:
                                    //WriteCentered("\nShopping Menu\n\n1. Search products\n2. Cart a product\n" +
                                    //    "3. View Orders\n4. Update Order Status\n5. View Products\n6. Review a product\n" +
                                    //    "(Press 0 to go back)\nEnter Choice : ");

                                    WriteCentered("Shopping Menu\n");
                                    WriteCentered("1. Search products");
                                    WriteCentered("2. Cart a product");
                                    WriteCentered("3. View Orders");
                                    WriteCentered("4. Update Order Status");
                                    WriteCentered("5. View Products");
                                    WriteCentered("6. Review a product");
                                    WriteCentered("(Press 0 to go back)\n");
                                    int c3 = int.Parse(ReadCentered("Enter Choice : "));
                                    if (c3 == 0) { break; }
                                    else if (c3 == 1)
                                    {
                                        //WriteCentered("Enter keyword : ");
                                        string keyword = ReadCentered("Enter keyword : ");
                                        List<Product> foundprods = productService.SearchProduct(keyword);
                                        productService.DisplayProducts(foundprods);

                                        //WriteCentered("\nEnter product id to add to cart: ");

                                        int oid = int.Parse(ReadCentered("Enter product id to add to cart : "));

                                        foreach (Product product in foundprods)
                                        {
                                            if (product.ProductId == oid)
                                            {
                                                //WriteCentered("Enter quantity :");
                                                int q = int.Parse(ReadCentered("Enter quantity : "));
                                                cartService.AddToCart(user.UserId, product, q);
                                            }
                                        }
                                    }
                                    else if (c3 == 2)
                                    {
                                        List<Product> allprods = productService.GetAllProducts();
                                        //WriteCentered("\nEnter product id to add to cart: ");
                                        productService.DisplayProducts(allprods);

                                        int oid = int.Parse(ReadCentered("Enter product id to add to cart : "));

                                        foreach (Product product in allprods)
                                        {
                                            if (product.ProductId == oid)
                                            {
                                                WriteCentered("Enter quantity : ");
                                                int q = int.Parse(ReadCentered(""));
                                                cartService.AddToCart(user.UserId, product, q);
                                            }
                                        }
                                    }
                                    else if (c3 == 3)
                                    {
                                        orderService.ViewOrders();
                                    }
                                    else if (c3 == 4)
                                    {
                                        //WriteCentered("Enter order id to update status : ");
                                        int oid = int.Parse(ReadCentered("Enter order id to update status : "));
                                        orderService.UpdateOrderStatus(oid);
                                    }
                                    else if (c3 == 5)
                                    {
                                        while (true)
                                        {
                                            //WriteCentered("\n1. View all products\n2. Sort by price\n" +
                                            //    "3. View by category\n4. View Reviews of a product\n" +
                                            //    "(Press 0 to go back)\nEnter choice : ");

                                            WriteCentered("View Products\n");
                                            WriteCentered("1. View all products");
                                            WriteCentered("2. Sort by price");
                                            WriteCentered("3. View by category");
                                            WriteCentered("4. View Reviews of a product");
                                            WriteCentered("(Press 0 to go back)\n");
                                            int v = int.Parse(ReadCentered("Enter choice : "));

                                            if (v == 0) { break; }

                                            if (v == 1)
                                            {
                                                productService.DisplayProducts(productService.GetAllProducts());
                                            }
                                            else if (v == 2)
                                            {
                                                productService.DisplayProducts(productService.SortProductsByPrice(true));
                                                //WriteCentered("Press\n 'a' for increasing order (default)\n 'd' for decreasing order\n");

                                                WriteCentered("Press 'a' for increasing order (default)");
                                                WriteCentered("Press 'd' for decreasing order");
                                                bool opt = ReadCentered("Enter mode : ") == "a" ? true : false;
                                                productService.DisplayProducts(productService.SortProductsByPrice(opt));
                                            }
                                            else if (v == 3)
                                            {
                                                //WriteCentered("Enter valid category : ");
                                                string cat = ReadCentered("Enter valid category : ");
                                                productService.DisplayProducts(productService.GetProductsByCategory((Category)Enum.Parse(typeof(Category), cat)));

                                            }
                                            else if (v == 4)
                                            {
                                                //WriteCentered("Enter product id : ");
                                                int prid = int.Parse(ReadCentered("Enter product id : "));
                                                Product p = productService.GetAllProducts().Find(p => p.ProductId == prid);
                                                reviewService.ShowReview(p);
                                            }
                                        }
                                    }
                                    else if (c3 == 6)
                                    {
                                        //WriteCentered("Enter product id to review : ");
                                        int prid = int.Parse(ReadCentered("Enter product id to review : "));
                                        Product p = productService.GetAllProducts().Find(p => p.ProductId == prid);
                                        //WriteCentered("Select review type :\n1. Critical\n2. NotBad\n3. Good\n4. VeryGood\n5. Excellent\nEnter choice : ");
                                        WriteCentered("Select review type :");
                                        WriteCentered("1. Critical");
                                        WriteCentered("2. NotBad");
                                        WriteCentered("3. Good");
                                        WriteCentered("4. VeryGood");
                                        WriteCentered("5. Excellent");
                                        int rt = int.Parse(ReadCentered("Enter choice : "));
                                        //WriteCentered("Enter review text : ");
                                        string reviewtext = ReadCentered("Enter review text : ");
                                        reviewService.AddReview((Customer)user, p, (ReviewType)rt, reviewtext);
                                    }
                                    break;

                                case 2:
                                    //WriteCentered("\nCart Menu\n1. View Cart\n2. Remove products from cart\nEnter choice : ");
                                    WriteCentered("Cart Menu\n");
                                    WriteCentered("1. View Cart");
                                    WriteCentered("2. Remove products from cart");

                                    int cc = int.Parse(ReadCentered("Enter choice : "));
                                    if (cc == 1)
                                    {
                                        cartService.ViewCart(user.UserId, orderService);
                                    }
                                    else if (cc == 2)
                                    {
                                        //WriteCentered("\nEnter product id to remove product from cart  : ");
                                        int pid = int.Parse(ReadCentered("Enter product id to remove product from cart  : "));
                                        if (pid != 0)
                                            cartService.RemoveFromCart(user.UserId, pid);
                                    }
                                    break;

                                case 3:
                                    user.DisplayDetails();
                                    break;
                            }
                        }

                    }
                    else if (user.Role == "Admin")
                    {
                        WriteCentered("Hi ");
                    }

                }
                else if (c1 == 0)
                {
                    WriteCentered("\nThank you !\n");
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            WriteCentered($"{ex.Message}");
        }
    }
}
