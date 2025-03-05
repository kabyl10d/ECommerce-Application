using ECommerceAppFinal.Exceptions;
class Program
{
    private static void WriteCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = Math.Max((windowWidth - textLength) / 2, 0);
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
    public static void Main()
    {
        UserService userService = new UserService();
        ProductService productService = new ProductService();
        OrderService orderService = new OrderService();
        CartService cartService = new CartService();
        ReviewService reviewService = new ReviewService();



        //WriteCentered("*************************************");
        //WriteCentered("****** WELCOME TO HeheCOMMERCE ******");
        //WriteCentered("*************************************");

        while (true)//login register loop
        {
        r1:
            //Console.Clear();
            try
            {

                WriteCentered("*************************************");
                WriteCentered("****** WELCOME TO HeheCommerce ******");
                WriteCentered("*************************************\n");
                WriteCentered("1. Register");
                WriteCentered("2. Login");
                WriteCentered("(Press 0 to Exit)");

                string sc1 = ReadCentered("Enter Choice :");

                if (sc1 == "" || sc1 != "0" && sc1 != "1" && sc1 != "2")
                {
                    Console.Clear();
                    throw new InvalidChoiceException("Invalid choice! Try again.");
                }
                else
                {
                    int c1 = int.Parse(sc1);

                    if (c1 == 1) //login register menu
                    {
                        Console.Clear();
                        WriteCentered("REGISTER AS USER\n");
                    un:
                        string rusername = ReadCentered("Enter Username : ");
                        try
                        {
                            if (string.IsNullOrEmpty(rusername))
                            {
                                //WriteCentered("Username cannot be empty! Try again.\n");
                                //Console.Clear();
                                throw new InvalidUsernameException();
                            }
                            if (UserService.users.Exists(u => u.Username == rusername))
                            {
                                throw new DuplicateUsernameException();

                                //WriteCentered("Username already exists!");
                                //goto un;
                            }
                        }
                        catch (InvalidUsernameException ex)
                        {
                            WriteCentered(ex.Message);
                            goto un;
                        }
                        catch (DuplicateUsernameException ex)
                        {
                            WriteCentered(ex.Message);
                            goto un;
                        }

                    mail:

                        string rmail = ReadCentered("Enter Mail Address : ");

                        try
                        {
                            if (string.IsNullOrEmpty(rmail))
                            {
                                throw new InvalidMailException();
                            }
                            if (!UserService.ValidEmail(rmail))
                            {
                                throw new InvalidMailException("Invalid mail address! Try again.\n");

                            }
                            if (UserService.users.Exists(u => u.Mailid == rmail))
                            {
                                throw new DuplicateMailException();
                            }
                        }
                        catch (InvalidMailException ex)
                        {
                            WriteCentered(ex.Message);
                            goto mail;
                        }
                        catch (DuplicateMailException ex)
                        {
                            WriteCentered(ex.Message);
                        }
                    pass:
                        string rpasswd = ReadCentered("Enter Password : ");
                        try
                        {
                            if (string.IsNullOrEmpty(rpasswd))
                            {
                                throw new InvalidPasswordException();
                            }
                            else if (!UserService.ValidPassword(rpasswd))
                            {
                                throw new InvalidPasswordException("At least 8 characters, including at least one uppercase letter, one lowercase letter, one digit, and one special character, with no spaces");
                            }
                            if (!rpasswd.Equals(ReadCentered("Confirm password : ")))
                            {
                                throw new InvalidPasswordException("Passwords do not match! Try again.");

                            }
                        }
                        catch (InvalidPasswordException ex)
                        {
                            WriteCentered(ex.Message);
                            goto pass;
                        }
                    ph:

                        string rphone = ReadCentered("Enter mobile number : ");
                        try
                        {
                            if (string.IsNullOrEmpty(rphone) || !UserService.ValidPhoneNumber(rphone))
                            {
                                throw new InvalidPhoneNumberException();
                            }
                        }
                        catch (InvalidPhoneNumberException ex)
                        {
                            WriteCentered(ex.Message);
                            goto ph;
                        }

                    role:
                        string rrole = ReadCentered("Enter Role (merchant / customer) : ");
                        try
                        {
                            if (string.IsNullOrEmpty(rrole) || (rrole.ToLower() != "merchant" && rrole.ToLower() != "customer"))
                            {
                                throw new InvalidRoleException();
                            }
                        }
                        catch (InvalidRoleException ex)
                        {
                            WriteCentered(ex.Message);
                            goto role;
                        }
                        if (rrole == "customer")
                        {
                        upiid:
                            string upiid = ReadCentered("Enter UPI ID : ");
                            try
                            {
                                if (!UserService.ValidUpiId(upiid))
                                {
                                    throw new InvalidUpiException();
                                }
                            }
                            catch (InvalidUpiException ex)
                            {
                                WriteCentered(ex.Message);
                                goto upiid;
                            }
                        upipin:
                            int upipin = int.Parse(ReadCentered("Enter the 6 digit UPI PIN : "));
                            try
                            {
                                if (upipin > 999999 || upipin < 100000)
                                {
                                    throw new InvalidUpiException("Invalid UPI PIN! Try again.");
                                }
                            }
                            catch (InvalidUpiException ex)
                            {
                                WriteCentered(ex.Message);
                                goto upipin;
                            }
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
                        //Console.Clear();
                        WriteCentered("");
                        WriteCentered("LOGIN\n");


                        string runmail = ReadCentered("Enter Username / Mail Address : ");
                        string rpasswd = ReadCentered("Enter Password : ");

                        User user = userService.Login(runmail, rpasswd);
                        if (user == null) { goto retry; }

                        if (user.Role == "Merchant")
                        {
                            // productService.DisplayProducts(productService.GetAllProducts());
                            //Console.Clear();

                            while (true)
                            {
                            //Console.Clear();
                            c2:
                                WriteCentered("");
                                WriteCentered("Merchant Menu \n");
                                WriteCentered("1. Add Products");
                                WriteCentered("2. Delete Products");
                                WriteCentered("3. View Products added by this merchant");
                                WriteCentered("4. View Profile");
                                WriteCentered("(Press 0 to Logout)");

                                //int c2 = int.Parse(ReadCentered("Enter choice : "));
                                string sc2 = ReadCentered("Enter Choice :");
                                if (sc2 == "")
                                {
                                    Console.Clear();
                                    throw new InvalidChoiceException("Choice can't be empty. Try again.");
                                }
                                int c2 = int.Parse(sc2);

                                try
                                {
                                    if (c2 < 0 || c2 > 4)
                                    {
                                        throw new InvalidChoiceException("Invalid choice! Try again.");
                                    }
                                }
                                catch (InvalidChoiceException ex)
                                {
                                    WriteCentered(ex.Message);
                                    goto c2;
                                }

                                if (c2 == 0) { Console.Clear(); break; }
                                switch (c2)//merchant menu
                                {
                                    case 1:
                                        //Console.Clear();
                                        //WriteCentered("Add Product Details\n\nEnter product name : ");
                                        WriteCentered("");
                                        WriteCentered("Add Product Details\n");
                                    pname:
                                        string pname = ReadCentered("Enter product name : ");
                                        try
                                        {
                                            if (string.IsNullOrEmpty(pname))
                                            {
                                                throw new InvalidProductDetailsException();
                                            }
                                        }
                                        catch (InvalidProductDetailsException ex)
                                        {
                                            WriteCentered(ex.Message);
                                            goto pname;
                                        }
                                    price:
                                        //double price = double.Parse(ReadCentered("Enter price : "));
                                        string sprice = ReadCentered("Enter price :");
                                        try
                                        {

                                            if (sprice == "")
                                            {
                                                throw new InvalidProductDetailsException("Invalid price amount. Try again.");
                                            }

                                            if (int.Parse(sprice) < 0)
                                            {
                                                throw new InvalidProductDetailsException("Invalid price amount. Try again.");
                                            }
                                        }
                                        catch (InvalidProductDetailsException ex)
                                        {
                                            WriteCentered(ex.Message);
                                            goto price;
                                        }
                                        int price = int.Parse(sprice);

                                    stock:
                                        //int stock = int.Parse(ReadCentered("Enter available count in stock : "));
                                        string sstock = ReadCentered("Enter available count in stock : ");

                                        try
                                        {

                                            if (sstock == "")
                                            {
                                                throw new InvalidProductDetailsException("Invalid price amount. Try again.");
                                            }
                                            if (int.Parse(sstock) < 0)
                                            {
                                                throw new InvalidProductDetailsException("Invalid stock amount. Try again.");
                                            }
                                        }
                                        catch (InvalidProductDetailsException ex)
                                        {
                                            WriteCentered(ex.Message);
                                            goto stock;
                                        }
                                        int stock = int.Parse(sstock);

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


                                        //int catn = int.Parse(ReadCentered("Enter category number : "));
                                        string scat = ReadCentered("Enter category number : ");

                                        try
                                        {
                                            if (scat == "")
                                            {
                                                throw new InvalidProductDetailsException("Invalid price amount. Try again.");
                                            }

                                            if (int.Parse(scat) < 1 || int.Parse(scat) > 8)
                                            {
                                                throw new InvalidProductDetailsException("Invalid category number! Try again.");
                                                //goto catn;
                                            }
                                        }
                                        catch (InvalidProductDetailsException ex)
                                        {
                                            WriteCentered(ex.Message);
                                            goto catn;
                                        }
                                        int catn = int.Parse(scat);

                                        Category cat = (Category)catn;

                                        productService.AddProduct((Merchant)user, pname, price, stock, cat);
                                        WriteCentered("");

                                        break;

                                    case 2:
                                        //Console.Clear();
                                        WriteCentered("\n*** Products Inventory ***\n");
                                        if (productService.DisplayProducts(productService.GetAllProducts()))
                                        {

                                            WriteCentered("Delete a Product\n");
                                        //WriteCentered("");
                                        pid:
                                            string pid = ReadCentered("Enter product id (0 to back): ");
                                            try
                                            {
                                                if (pid == "0")
                                                {
                                                    break;
                                                }
                                                if (string.IsNullOrEmpty(pid))
                                                {
                                                    throw new InvalidProductDetailsException("Invalid product id! Try again.");

                                                }
                                                if (int.Parse(pid) < 1)
                                                {
                                                    throw new InvalidProductDetailsException("Invalid product id! Try again.");
                                                }

                                            }
                                            catch (InvalidProductDetailsException ex)
                                            {
                                                WriteCentered(ex.Message);
                                                goto pid;
                                            }
                                            Merchant m1 = (Merchant)user;
                                            WriteCentered("");
                                            try
                                            {
                                                if (!productService.RemoveProductById(int.Parse(pid), m1))
                                                {
                                                    throw new ProductNotFoundException("Product not found.");
                                                }
                                            }
                                            catch (ProductNotFoundException ex)
                                            {
                                                WriteCentered(ex.Message);

                                            }
                                        }

                                        break;

                                    case 3:
                                        Merchant m2 = (Merchant)user;
                                        //Console.Clear();
                                        WriteCentered("");
                                        WriteCentered($"Products added by {m2.Username}\n");
                                        productService.DisplayProducts(m2.products);
                                        break;

                                    case 4:
                                        Console.Clear();
                                        WriteCentered("My Profile - ");
                                        user.DisplayDetails();
                                        break;
                                }
                            }
                        }
                        else if (user.Role == "Customer")
                        {
                            //Console.Clear();

                            //WriteCentered("\n*** Products Inventory ***\n");
                            //productService.DisplayProducts(productService.GetAllProducts());

                            while (true)
                            {
                                WriteCentered("\n*** Products Inventory ***\n");
                                productService.DisplayProducts(productService.GetAllProducts());
                            //WriteCentered("\nCustomer Menu\n\n1. Shopping\n2. Cart and Checkout\n3. View Profile\n" +
                            //    "(Press 0 to Exit)\n\nEnter Choice : ");
                            c2:
                                WriteCentered("Customer Menu\n");
                                WriteCentered("1. Shopping");
                                WriteCentered("2. Cart and Checkout");
                                WriteCentered("3. View Profile");
                                WriteCentered("(Press 0 to Logout)\n");
                                //int c2 = int.Parse(ReadCentered("Enter Choice : "));
                                string sc2 = ReadCentered("Enter Choice :");
                                if (sc2 == "")
                                {
                                    Console.Clear();
                                    throw new InvalidChoiceException("Choice can't be empty. Try again.");
                                }
                                int c2 = int.Parse(sc2);
                                try
                                {
                                    if (c2 < 0 || c2 > 3)
                                    {
                                        throw new InvalidChoiceException("Invalid choice! Try again.");
                                    }
                                }
                                catch (InvalidChoiceException ex)
                                {
                                    WriteCentered(ex.Message);
                                    goto c2;
                                }
                                if (c2 == 0) { Console.Clear(); break; }


                                switch (c2)
                                {
                                    case 1:
                                    //WriteCentered("\nShopping Menu\n\n1. Search products\n2. Cart a product\n" +
                                    //    "3. View Orders\n4. Update Order Status\n5. View Products\n6. Review a product\n" +
                                    //    "(Press 0 to go back)\nEnter Choice : ");
                                    //Console.Clear();
                                    c3:
                                        WriteCentered("");
                                        WriteCentered("Shopping Menu\n");
                                        WriteCentered("1. Search and Order a product");
                                        WriteCentered("2. View Orders");
                                        WriteCentered("3. Update Order Status");
                                        WriteCentered("4. View Products");
                                        WriteCentered("5. Review a product");
                                        WriteCentered("(Press 0 to go back)\n");
                                        string sc3 = ReadCentered("Enter Choice : ");
                                        try
                                        {
                                            if (string.IsNullOrEmpty(sc3))
                                            {
                                                Console.Clear();
                                                throw new InvalidChoiceException("Invalid choice! Try again.");
                                            }
                                            if (int.Parse(sc3) < 0 || int.Parse(sc3) > 5)
                                            {
                                                 Console.Clear();

                                                throw new InvalidChoiceException("Invalid choice! Try again.");
                                            }
                                        }
                                        catch (InvalidChoiceException ex)
                                        {
                                            WriteCentered(ex.Message);
                                            goto c3;
                                        }
                                        int c3 = int.Parse(sc3);
                                        if (c3 == 0) { Console.Clear(); break; }
                                        else if (c3 == 1)
                                        {
                                            //Console.Clear();
                                            Customer cust = (Customer)user;
                                            //WriteCentered("\nEnter product id to add to cart: ");
                                            productService.SearchAndBuy(cust, cartService);
                                            break;

                                        }
                                        else if (c3 == 2)
                                        {
                                            orderService.ViewOrders();
                                        }
                                        else if (c3 == 3)
                                        {
                                        //WriteCentered("Enter order id to update status : ");
                                        oid:
                                            if (orderService.ViewOrders())
                                            {
                                                string oid = ReadCentered("Enter order id to update status (0 to back): ");
                                                try
                                                {
                                                    if (oid == "0")
                                                    {
                                                        break;
                                                    }
                                                    if (string.IsNullOrEmpty(oid))
                                                    {
                                                        throw new InvalidOrderDetailsException();

                                                    }
                                                    if (int.Parse(oid) < 1)
                                                    {
                                                        throw new InvalidOrderDetailsException();
                                                    }
                                                    else
                                                    {
                                                        orderService.UpdateOrderStatus(int.Parse(oid));
                                                    }
                                                }
                                                catch (InvalidOrderDetailsException ex)
                                                {
                                                    WriteCentered(ex.Message);
                                                    goto oid;
                                                }
                                            }
                                        }
                                        else if (c3 == 4)
                                        {
                                            //Console.Clear();
                                            while (true)
                                            {
                                            //WriteCentered("\n1. View all products\n2. Sort by price\n" +
                                            //    "3. View by category\n4. View Reviews of a product\n" +
                                            //    "(Press 0 to go back)\nEnter choice : ");
                                            v:
                                                try
                                                {
                                                    WriteCentered("View Products\n");
                                                    WriteCentered("1. View all products");
                                                    WriteCentered("2. Sort by price");
                                                    WriteCentered("3. View by category");
                                                    WriteCentered("4. View Reviews of a product");
                                                    WriteCentered("(Press 0 to go back)\n");
                                                    //int v = int.Parse(ReadCentered("Enter choice : "));
                                                    string sv = ReadCentered("Enter Choice :");
                                                    if (sv == "")
                                                    {
                                                        Console.Clear();
                                                        throw new InvalidChoiceException("Choice can't be empty. Try again.");
                                                    }
                                                    int v = int.Parse(sv);
                                                    if (v < 0 || v > 4)
                                                    {
                                                        throw new InvalidChoiceException("Invalid choice! Try again.");
                                                    }


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
                                                        bool opt = ReadCentered("Enter mode : ") == "a";
                                                        productService.DisplayProducts(productService.SortProductsByPrice(opt));
                                                    }
                                                    else if (v == 3)
                                                    {
                                                        //WriteCentered("Enter valid category : ");
                                                        WriteCentered("1. Electronics");
                                                        WriteCentered("2. Mobiles");
                                                        WriteCentered("3. HomeKitchen");
                                                        WriteCentered("4. Fashion");
                                                        WriteCentered("5. Beauty");
                                                        WriteCentered("6. Health");
                                                        WriteCentered("7. BabyProducts");
                                                        WriteCentered("8. Stationary");
                                                        string cat = ReadCentered("Enter valid category (0 to back) : ");

                                                        if (cat == "0")
                                                        {
                                                            break;
                                                        }
                                                        productService.DisplayProducts(productService.GetProductsByCategory((Category)Enum.Parse(typeof(Category), cat)));

                                                    }
                                                    else if (v == 4)
                                                    {
                                                        //WriteCentered("Enter product id : ");

                                                        int prid = int.Parse(ReadCentered("Enter product id (0 to back) : "));
                                                        if (prid == 0)
                                                        {
                                                            break;
                                                        }
                                                        Product p = productService.GetAllProducts().Find(p => p.ProductId == prid);
                                                        reviewService.ShowReview(p);
                                                    }
                                                }
                                                catch (InvalidChoiceException ex)
                                                {
                                                    WriteCentered(ex.Message);
                                                    goto v;
                                                }
                                            }
                                        }
                                        else if (c3 == 5)
                                        {
                                            //WriteCentered("Enter product id to review : ");
                                            if (productService.DisplayProducts(productService.GetAllProducts()))
                                            {
                                                int prid = int.Parse(ReadCentered("Enter product id to review (0 to back) : "));
                                                if (prid == 0)
                                                {
                                                    break;
                                                }
                                                Product p = productService.GetAllProducts().Find(p => p.ProductId == prid);
                                                //WriteCentered("Select review type :\n1. Critical\n2. NotBad\n3. Good\n4. VeryGood\n5. Excellent\nEnter choice : ");
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
                                                    reviewService.AddReview((Customer)user, p, (ReviewType)rt, reviewtext);
                                                }
                                                else
                                                {
                                                    WriteCentered("Product not found!");
                                                }
                                            }

                                        }
                                        break;

                                    case 2:
                                        //WriteCentered("\nCart Menu\n1. View Cart\n2. Remove products from cart\nEnter choice : ");
                                        // Console.Clear();
                                        WriteCentered("");
                                        WriteCentered("Cart Menu\n");
                                        WriteCentered("1. View Cart");
                                        WriteCentered("2. Remove products from cart");
                                        WriteCentered("Press 0 to leave");

                                        int cc = int.Parse(ReadCentered("Enter choice : "));
                                        if (cc == 1)
                                        {
                                            cartService.ViewCart(user.UserId, orderService);
                                        }
                                        else if (cc == 2)
                                        {
                                            //WriteCentered("\nEnter product id to remove product from cart  : ");
                                            int pid = int.Parse(ReadCentered("Enter product id to remove product from cart  (0 to back)): "));
                                            if (pid == 0)
                                            {
                                                break;
                                            }
                                            cartService.RemoveFromCart(user.UserId, pid);

                                        }
                                        else
                                        {
                                            Console.Clear();
                                            break;
                                        }
                                        break;

                                    case 3:
                                        Console.Clear();
                                        WriteCentered("My Profile - ");

                                        user.DisplayDetails();
                                        break;
                                }
                            }

                        }
                    }
                    else if (c1 == 0)
                    {
                        WriteCentered("");
                        WriteCentered("Thank you! ;-) \n");
                        return;
                    }
                }
            }
            catch (InvalidChoiceException ex)
            {

                WriteCentered(ex.Message);
                goto r1;
            }
            //    finally
            //    {
            //        WriteCentered("Application executed.");
            //    }
        }

    }

}

