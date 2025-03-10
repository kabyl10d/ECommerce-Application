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


        userService.Register("admin", "admin@gmail.com", "Admin@123", "9344776261", "admin" );
        userService.Register("merchant", "merchant@gmail.com", "Mer@1234", "9344776261", "merchant" );
        userService.Register("customer", "customer@gmail.com", "Cus@1234", "9344776261", "customer");


        Console.Clear();
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
                    reg:
                    if (c1 == 1) //login register menu
                    {
                        Console.Clear();
                        WriteCentered("REGISTER AS USER");
                        WriteCentered("(0 to back)");
                    un:
                        string rusername = ReadCentered("Enter Username : ");
                        try
                        {
                            if (rusername == "0")
                            {
                                Console.Clear();
                                goto r1;
                            }
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
                            if (rmail == "0")
                            {
                                Console.Clear();
                                goto r1;
                            }
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
                            if (rpasswd == "0")
                            {
                                Console.Clear();
                                goto r1;
                            }
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
                            if (rphone == "0")
                            {
                                Console.Clear();
                                goto r1;
                            }
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
                            if (rrole == "0")
                            {
                                Console.Clear();
                                goto r1;
                            }
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
                        userService.Register(rusername, rmail, rpasswd, rphone, rrole);
                        

                    }
                    else if (c1 == 2)
                    {
                    retry:
                        //Console.Clear();
                        WriteCentered("");
                        WriteCentered("LOGIN");
                        WriteCentered("(0 to back)");
                        WriteCentered("");


                        string runmail = ReadCentered("Enter Username / Mail Address : ");
                        if(runmail == "0") { Console.Clear(); goto r1; }
                        if(string.IsNullOrEmpty(runmail))
                        {
                            WriteCentered("Invalid input! Try again.");
                            goto retry;
                        }
                    retry1:
                        string rpasswd = ReadCentered("Enter Password : ");
                        if(rpasswd == "0") { Console.Clear(); goto r1; }
                        if(string.IsNullOrEmpty(rpasswd))
                        {
                            WriteCentered("Invalid input! Try again.");
                            goto retry1;
                        }

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
                                WriteCentered("\n*** Products Inventory ***\n");
                                productService.DisplayProducts(productService.GetAllProducts());
                                WriteCentered("");
                                WriteCentered("Merchant Menu \n");
                                WriteCentered("1. Add Products");
                                WriteCentered("2. Delete Products");
                                WriteCentered($"3. View Products added by {user.Username}");
                                WriteCentered("4. Update stock of a product");
                                WriteCentered("5. Recieve order and update status");
                                WriteCentered("5. View Profile");
                                WriteCentered("(Press 0 to Logout)");

                                cc2:
                                //int c2 = int.Parse(ReadCentered("Enter choice : "));
                                string sc2 = ReadCentered("Enter Choice :");
                                try
                                {
                                    if (sc2 == "")
                                    {
                                        //Console.Clear();
                                        throw new InvalidChoiceException("Choice can't be empty. Try again.");
                                    }
                                }
                                catch (InvalidChoiceException ex)
                                {
                                    WriteCentered(ex.Message);
                                    goto cc2;
                                }
                                int c2 = int.Parse(sc2);

                                try
                                {
                                    if (c2 < 0 || c2 > 5)
                                    {
                                        throw new InvalidChoiceException("Invalid choice! Try again.");
                                    }
                                }
                                catch (InvalidChoiceException ex)
                                {
                                    WriteCentered(ex.Message);
                                    goto cc2;
                                }

                                if (c2 == 0) { Console.Clear(); break; }
                                //merchant menu
                                switch (c2)
                                {
                                    //add product
                                    case 1:
                                        //Console.Clear();
                                        //WriteCentered("Add Product Details\n\nEnter product name : ");

                                        WriteCentered("");
                                        WriteCentered("Add Product Details");
                                        WriteCentered("(0 to back)");
                                    pname:
                                        string pname = ReadCentered("Enter product name : ");
                                        try
                                        {
                                            if(pname == "0")
                                            {
                                                Console.Clear();
                                                goto c2;
                                            }
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
                                            if(sprice == "0")
                                            {
                                                Console.Clear();
                                                goto c2;
                                            }
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
                                            if (sstock == "0")
                                            {
                                                Console.Clear();
                                                goto c2;
                                            }

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
                                            if (scat == "0")
                                            {
                                                Console.Clear();
                                                goto c2;
                                            }

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

                                    //delete product
                                    case 2:
                                        Merchant m1 = (Merchant)user;

                                        Console.Clear();
                                        WriteCentered("");
                                        WriteCentered($"\n*** Products Added by {m1.Username} ***\n");
                                        if (productService.DisplayProducts(m1.products))//productService.DisplayProducts(productService.GetAllProducts()))
                                        {

                                            WriteCentered("Delete a Product\n");
                                        //WriteCentered("");
                                        pid:
                                            string pid = ReadCentered("Enter product id (0 to back): ");
                                            try
                                            {
                                                if (pid == "0")
                                                {
                                                    goto c2;
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
                                                goto pid;
                                            }
                                        }

                                        break;

                                    //view products added by merchant
                                    case 3:
                                        Merchant m3 = (Merchant)user;
                                        Console.Clear();
                                        WriteCentered("");
                                        WriteCentered($"Products added by {m3.Username}\n");
                                        productService.DisplayProducts(m3.products);
                                        WriteCentered("");

                                        break;

                                    //update stock of a product
                                    case 4:
                                        Merchant m2 = (Merchant)user;

                                        Console.Clear();
                                        WriteCentered("");
                                        WriteCentered($"\n*** Products Added by {m2.Username} ***\n");
                                        if (productService.DisplayProducts(m2.products))//productService.DisplayProducts(productService.GetAllProducts()))
                                        {

                                            WriteCentered("Update stock of a product\n");
                                        //WriteCentered("");
                                        pid:
                                            string pid = ReadCentered("Enter product id (0 to back): ");
                                            try
                                            {
                                                if (pid == "0")
                                                {
                                                    goto c2;
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
                                            WriteCentered("");
                                            try
                                            {
                                                if (!productService.UpdateProductStock(int.Parse(pid)))
                                                {
                                                    throw new ProductNotFoundException("Product not found.");
                                                }
                                             
                                            }
                                            catch (ProductNotFoundException ex)
                                            {
                                                WriteCentered(ex.Message);
                                                goto pid;
                                            }
                                        }
                                        break;

                                    case 5:
                                        Console.Clear();
                                        WriteCentered("");
                                        Merchant mm = (Merchant)user;
                                    oid:
                                        if (orderService.ViewOrdersForMerchant(mm))
                                        {

                                            string oid = ReadCentered("Enter order id to update status (0 to back): ");
                                            try
                                            {

                                                if (oid == "0")
                                                {
                                                    Console.Clear();
                                                    goto c2;

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
                                                opid:
                                                    string opid = ReadCentered("Enter product id from the order to update status (0 to back): ");
                                                    try
                                                    {
                                                        if (oid == "0")
                                                        {
                                                            Console.Clear();
                                                            goto oid;

                                                        }
                                                        if (string.IsNullOrEmpty(oid))
                                                        {
                                                            throw new InvalidOrderDetailsException("Enter valid id.");

                                                        }
                                                        if (int.Parse(oid) < 1)
                                                        {
                                                            throw new InvalidOrderDetailsException("Enter valid id.");
                                                        }
                                                    }
                                                    catch (InvalidOrderDetailsException ex)
                                                    {
                                                        WriteCentered(ex.Message);
                                                        goto opid;
                                                    }
                                                    orderService.RecieveOrder(int.Parse(oid), int.Parse(opid));
                                                    goto c2;
                                                }
                                                
                                            }
                                            catch (InvalidOrderDetailsException ex)
                                            {
                                                WriteCentered(ex.Message);
                                                goto oid;
                                            }
                                        }
                                        else
                                        {
                                            goto c2;
                                        }
                                        //break;
                                    //view profile of the merchant    
                                    case 6:
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
                                cc2:
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
                                try
                                {
                                    if (sc2 == "")
                                    {
                                        Console.Clear();
                                        throw new InvalidChoiceException("Choice can't be empty. Try again.");
                                    }
                                }
                                catch (InvalidChoiceException ex)
                                {
                                    WriteCentered(ex.Message);
                                    goto cc2;
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
                                    //shopping menu
                                    case 1:
                                    //WriteCentered("\nShopping Menu\n\n1. Search products\n2. Cart a product\n" +
                                    //    "3. View Orders\n4. Update Order Status\n5. View Products\n6. Review a product\n" +
                                    //    "(Press 0 to go back)\nEnter Choice : ");
                                    //Console.Clear();
                                    c3:
                                        productService.DisplayProducts(productService.GetAllProducts());

                                        WriteCentered("");
                                        WriteCentered("Shopping Menu\n");
                                        WriteCentered("1. Search and Order a product");
                                        WriteCentered("2. View Orders");
                                        WriteCentered("3. Update Order Status");
                                        WriteCentered("4. View Products");
                                        //WriteCentered("5. Review a product");
                                        //WriteCentered("6. View Reviews of a product");
                                        WriteCentered("(Press 0 to go back)\n");
                                        string sc3 = ReadCentered("Enter Choice : ");
                                        try
                                        {
                                            if (string.IsNullOrEmpty(sc3))
                                            {
                                                Console.Clear();
                                                throw new InvalidChoiceException("Invalid choice! Try again.");
                                            }
                                            if(sc3.Length > 1)
                                            {
                                                throw new InvalidChoiceException("Invalid choice! Try again.");
                                            }
                                            if (int.Parse(sc3) < 0 || int.Parse(sc3) > 4)
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
                                            if(!productService.SearchAndBuy(cust, cartService, reviewService))
                                            {
                                                Console.Clear();
                                                goto c3;
                                            }
                                            break;

                                        }
                                        else if (c3 == 2)
                                        {
                                            WriteCentered("");
                                            orderService.ViewOrders();
                                            WriteCentered("");
                                            goto c3;

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
                                                        Console.Clear();
                                                        goto c3;
                                                       
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
                                                        opid:
                                                        string opid = ReadCentered("Enter product id from the order to update status (0 to back): ");
                                                        try
                                                        {
                                                            if (oid == "0")
                                                            {
                                                                Console.Clear();
                                                                goto oid;

                                                            }
                                                            if (string.IsNullOrEmpty(oid))
                                                            {
                                                                throw new InvalidOrderDetailsException("Enter valid id.");

                                                            }
                                                            if (int.Parse(oid) < 1)
                                                            {
                                                                throw new InvalidOrderDetailsException("Enter valid id.");
                                                            }
                                                        }
                                                        catch (InvalidOrderDetailsException ex)
                                                        {
                                                            WriteCentered(ex.Message);
                                                            goto opid;
                                                        }
                                                        orderService.UpdateOrderStatus(int.Parse(oid),int.Parse(opid));
                                                        goto c3;
                                                    }
                                                }
                                                catch (InvalidOrderDetailsException ex)
                                                {
                                                    WriteCentered(ex.Message);
                                                    goto oid;
                                                }
                                            }
                                            else
                                            {
                                                goto c3;
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
                                                    //WriteCentered("4. View Reviews of a product");
                                                    WriteCentered("(Press 0 to go back)\n");
                                                    //int v = int.Parse(ReadCentered("Enter choice : "));
                                                    string sv = ReadCentered("Enter Choice :");
                                                    if (sv == "")
                                                    {
                                                        Console.Clear();
                                                        throw new InvalidChoiceException("Choice can't be empty. Try again.");
                                                    }
                                                    int v = int.Parse(sv);
                                                    if (v < 0 || v > 3)
                                                    {
                                                        throw new InvalidChoiceException("Invalid choice! Try again.");
                                                    }


                                                    if (v == 0) { Console.Clear();  goto c3; }

                                                    if (v == 1)
                                                    {
                                                        Console.Clear();

                                                        productService.DisplayProducts(productService.GetAllProducts());
                                                        //Console.Clear();
                                                    }
                                                    else if (v == 2)
                                                    {
                                                        productService.DisplayProducts(productService.SortProductsByPrice(true));
                                                        //WriteCentered("Press\n 'a' for increasing order (default)\n 'd' for decreasing order\n");
                                                        sort:
                                                        WriteCentered("Press 'a' for increasing order (default)");
                                                        WriteCentered("Press 'd' for decreasing order");
                                                        string opt1 = ReadCentered("Enter mode : ");
                                                        bool opt = true;
                                                        
                                                        if (opt1 == "a")
                                                        {
                                                            opt = true;
                                                        }
                                                        else if(opt1 == "d")
                                                        {
                                                            opt = false;
                                                        }
                                                        else
                                                        {
                                                            WriteCentered("Enter valid option.");
                                                            goto sort;
                                                        }
                                                        
                                                        Console.Clear();
                                                        productService.DisplayProducts(productService.SortProductsByPrice(opt));
                                                        //Console.Clear();
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
                                                            goto v;

                                                        }
                                                        Console.Clear();
                                                        productService.DisplayProducts(productService.GetProductsByCategory((Category)Enum.Parse(typeof(Category), cat)));
                                                        

                                                    }
                                                    //else if (v == 4)
                                                    //{
                                                    //    //WriteCentered("Enter product id : ");

                                                    //    int prid = int.Parse(ReadCentered("Enter product id (0 to back) : "));
                                                    //    if (prid == 0)
                                                    //    {
                                                    //        break;
                                                    //    }
                                                    //    Product p = productService.GetAllProducts().Find(p => p.ProductId == prid);
                                                    //    reviewService.ShowReview(p);
                                                    //}
                                                }
                                                catch (InvalidChoiceException ex)
                                                {
                                                    WriteCentered(ex.Message);
                                                    goto v;
                                                }
                                            }
                                        }
                                        //else if (c3 == 5)
                                        //{
                                        //    //WriteCentered("Enter product id to review : ");
                                        //    if (productService.DisplayProducts(productService.GetAllProducts()))
                                        //    {
                                        //        int prid = int.Parse(ReadCentered("Enter product id to review (0 to back) : "));
                                        //        if (prid == 0)
                                        //        {
                                        //            Console.Clear();
                                        //            goto c3;
                                        //        }
                                        //        Product p = productService.GetAllProducts().Find(p => p.ProductId == prid);
                                        //        //WriteCentered("Select review type :\n1. Critical\n2. NotBad\n3. Good\n4. VeryGood\n5. Excellent\nEnter choice : ");
                                        //        if (p != null)
                                        //        {
                                        //        rt:
                                        //            WriteCentered("Select review type :");
                                        //            WriteCentered("1. Critical");
                                        //            WriteCentered("2. NotBad");
                                        //            WriteCentered("3. Good");
                                        //            WriteCentered("4. VeryGood");
                                        //            WriteCentered("5. Excellent");
                                        //            int rt = int.Parse(ReadCentered("Enter choice : "));
                                        //            try
                                        //            {
                                        //                if (rt == 0)
                                        //                {
                                        //                    Console.Clear();
                                        //                    goto c3;
                                        //                }
                                        //                if (rt < 1 || rt > 5)
                                        //                {
                                        //                    throw new InvalidChoiceException("Invalid review choice! Try again.");
                                        //                }
                                        //            }
                                        //            catch (InvalidChoiceException ex)
                                        //            {
                                        //                WriteCentered(ex.Message);
                                        //                goto rt;
                                        //            }
                                        //            //WriteCentered("Enter review text : ");
                                        //            string reviewtext = ReadCentered("Enter review text : ");
                                        //            if (reviewtext == "0")
                                        //            {
                                        //                Console.Clear();
                                        //                goto c3;
                                        //            }
                                        //            reviewService.AddReview((Customer)user, p, (ReviewType)rt, reviewtext);
                                        //            goto c3;
                                        //        }
                                        //        else
                                        //        {
                                        //            WriteCentered("Product not found!");
                                        //            goto c3;
                                        //        }
                                        //    }

                                        //}
                                        //else if (c3 == 6)
                                        //{
                                        //    //WriteCentered("Enter product id : ");

                                        //    int prid = int.Parse(ReadCentered("Enter product id (0 to back) : "));
                                        //    if (prid == 0)
                                        //    {
                                        //        goto c3;
                                        //    }
                                        //    Product p = productService.GetAllProducts().Find(p => p.ProductId == prid);
                                        //    reviewService.ShowReview(p);
                                        //    goto c3;
                                        //}
                                        break;

                                    //cart menu
                                    case 2:

                                        //WriteCentered("\nCart Menu\n1. View Cart\n2. Remove products from cart\nEnter choice : ");
                                        // Console.Clear();
                                        cartmenu:
                                        WriteCentered("");
                                        WriteCentered("Cart Menu\n");
                                        WriteCentered("1. View Cart");
                                        WriteCentered("2. Modify cart");
                                        WriteCentered("Press 0 to leave");

                                        string cc = ReadCentered("Enter choice : ");
                                        try
                                        {
                                            if (string.IsNullOrEmpty(cc))
                                            {
                                                throw new InvalidChoiceException("Invalid choice! Try again.");
                                            }

                                        }
                                        catch(InvalidChoiceException ex)
                                        {
                                            WriteCentered(ex.Message);
                                            goto cartmenu;
                                        }

                                        if (cc == "0")
                                        {
                                            Console.Clear();
                                            break;
                                        }
                                        if (cc == "1")
                                        {
                                            cartService.ViewCart(user.UserId, orderService);
                                            goto cartmenu;
                                        }
                                        else if (cc == "2")
                                        {
                                        modmenu:
                                            WriteCentered("");
                                            WriteCentered("Modify Cart Menu\n");
                                            WriteCentered("1. Update count of the item");
                                            WriteCentered("2. Remove the item from the cart");
                                            WriteCentered("Press 0 to leave");
                                            string ccc = ReadCentered("Enter choice : ");
                                            if (ccc == "0")
                                            {
                                                Console.Clear();
                                                goto cartmenu;
                                            }
                                            else if (ccc == "1")
                                            {
                                                if (cartService.DisplayCartItems(user.UserId))
                                                {
                                                    string pid = ReadCentered("Enter product id to update count (0 to back) : ");
                                                    if (pid == "0")
                                                    {
                                                        goto modmenu;
                                                    }
                                                    string count = ReadCentered("Enter new count : ");
                                                    if (count == "0")
                                                    {
                                                        goto modmenu;
                                                    }
                                                    cartService.UpdateCart(user.UserId, int.Parse(pid), int.Parse(count));
                                                    goto modmenu;
                                                }
                                                else
                                                {
                                                    goto cartmenu;
                                                }


                                            }
                                            else if (ccc == "2")
                                            {
                                                remove:
                                                if (cartService.DisplayCartItems(user.UserId))
                                                {
                                                    //WriteCentered("\nEnter product id to remove product from cart  : ");
                                                    string pid = ReadCentered("Enter product id to remove product from cart  (0 to back)): ");
                                                    if (pid == "")
                                                    {
                                                        WriteCentered("Invalid choice! Try again.");
                                                        goto remove;
                                                    }
                                                    if (pid == "0")
                                                    {
                                                        goto modmenu;
                                                    }
                                                    cartService.RemoveFromCart(user.UserId,int.Parse(pid));
                                                    goto remove;
                                                }
                                                else
                                                {
                                                    goto cartmenu;
                                                }
                                               
                                            }

                                            else
                                            {
                                                Console.Clear();
                                                goto c3;
                                            }
                                        }
                                        break;

                                    //profile of the customer
                                    case 3:
                                        Console.Clear();
                                        WriteCentered("My Profile - ");
                                        user.DisplayDetails();
                                        break;
                                        goto c2;
 
                                }
                            }

                        }
                        else if (user.Role == "Admin")
                        {
                        adminmenu:
                            try
                            {


                                while (true)
                                {
                                    // WriteCentered("Welcome Admin !");
                                    WriteCentered("");
                                    WriteCentered("*** Product Inventory ***");
                                    WriteCentered("");
                                    WriteCentered($"1. Electronics ({productService.GetProductCountByCategory(Category.Electronics)})");
                                    WriteCentered($"2. Mobiles ({productService.GetProductCountByCategory(Category.Mobiles)})");
                                    WriteCentered($"3. HomeKitchen ({productService.GetProductCountByCategory(Category.HomeKitchen)})");
                                    WriteCentered($"4. Fashion ({productService.GetProductCountByCategory(Category.Fashion)})");
                                    WriteCentered($"5. Beauty ({productService.GetProductCountByCategory(Category.Beauty)})");
                                    WriteCentered($"6. Health ({productService.GetProductCountByCategory(Category.Health)})");
                                    WriteCentered($"7. BabyProducts ({productService.GetProductCountByCategory(Category.BabyProducts)})");
                                    WriteCentered($"8. Stationary ({productService.GetProductCountByCategory(Category.Stationary)})");
                                    WriteCentered("");

                                    string sc4 = ReadCentered("Select Category (0 to logout) : ");
                                    //sc4 != "0" && sc4 != "1" && sc4 != "2"
                                    if (sc4 == "0")
                                    {
                                        Console.Clear();
                                        break;
                                    }
                                    List<String> opts = ["1", "2", "3", "4", "5", "6", "7", "8"];
                                    if (sc4 == "" || !opts.Contains(sc4))
                                    {
                                        Console.Clear();
                                        throw new InvalidChoiceException("Invalid choice! Try again.");

                                    }
                                ar1:
                                    if (productService.GetProductsByCategory((Category)Enum.Parse(typeof(Category), sc4)).Count() == 0)
                                    {
                                        WriteCentered("");

                                        WriteCentered("No products found.");
                                        //goto ar1;
                                    }
                                    productService.DisplayProductNamesWithCount((Category)Enum.Parse(typeof(Category), sc4));
                                    WriteCentered("");
                                    WriteCentered($"1. View more details of a products in {(Category)Enum.Parse(typeof(Category), sc4)}");
                                   // WriteCentered($"2. Add products in {(Category)Enum.Parse(typeof(Category), sc4)}");
                                    WriteCentered($"2. Remove products in {(Category)Enum.Parse(typeof(Category), sc4)}");
                                    WriteCentered("");

                                    string catchoice = ReadCentered("Enter choice (0 to back) : ");
                                    WriteCentered("");
                                    if (catchoice == "0")
                                    {
                                        Console.Clear();
                                        continue;
                                    }
                                    //List<String> opts1 = ["1", "2", "3"];
                                    try
                                    {
                                        if (catchoice == "")// || !opts1.Contains(catchoice))
                                        {
                                            Console.Clear();
                                            throw new InvalidChoiceException("Invalid choice! Try again.");
                                        }
                                    }
                                    catch (InvalidChoiceException ex)
                                    {
                                        WriteCentered(ex.Message);
                                        goto ar1;
                                    }
                                    try
                                    {
                                        switch (catchoice)
                                        {
                                            case "1":
                                                //productService.DisplayProducts(productService.GetProductsByCategory((Category)Enum.Parse(typeof(Category), sc4)));
                                                if (productService.GetProductsByCategory((Category)Enum.Parse(typeof(Category), sc4)).Count() == 0)
                                                {
                                                    //WriteCentered("");
                                                    //WriteCentered("No products found.");
                                                    goto ar1;
                                                }
                                                //productService.DisplayProductNamesWithCount((Category)Enum.Parse(typeof(Category), sc4));
                                                ss1:
                                                try
                                                {
                                                    WriteCentered("");
                                                    string pname = ReadCentered("Enter product name for more details. (0 to back) : ");
                                                    if (pname == "0")
                                                    {
                                                        Console.Clear();
                                                        goto ar1;
                                                    }
                                                    if (pname == "")
                                                    {
                                                        throw new InvalidChoiceException("Invalid choice! Try again.");
                                                    }
                                                    if(!productService.DisplayProductsWithDetails(pname, (Category)Enum.Parse(typeof(Category), sc4)))
                                                    {
                                                        goto ss1;
                                                    }
                                                    WriteCentered("");
                                                    ss:
                                                    try
                                                    {
                                                        WriteCentered("");
                                                        WriteCentered("1. To delete a product ");
                                                        WriteCentered("2. To go back ");
                                                        string yn = ReadCentered("Enter choice : ");
                                                        if (yn == "1")
                                                        {
                                                        pid1:
                                                            string pid1 = ReadCentered("Enter product id to delete (0 to back) : ");

                                                            try
                                                            {
                                                                if (pid1 == "0")
                                                                {
                                                                    goto ss;
                                                                }
                                                                if (string.IsNullOrEmpty(pid1))
                                                                {
                                                                    throw new InvalidProductDetailsException("Invalid product id! Try again.");

                                                                }
                                                                if (int.Parse(pid1) < 1)
                                                                {
                                                                    throw new InvalidProductDetailsException("Invalid product id! Try again.");
                                                                }

                                                            }
                                                            catch (InvalidProductDetailsException ex)
                                                            {
                                                                WriteCentered(ex.Message);
                                                                goto pid1;
                                                            }
                                                            WriteCentered("");
                                                            try
                                                            {
                                                                if (!productService.DeleteProduct(int.Parse(pid1)))
                                                                {
                                                                    throw new ProductNotFoundException("Product not found.");
                                                                }
                                                            }
                                                            catch (ProductNotFoundException ex)
                                                            {
                                                                WriteCentered(ex.Message);
                                                                goto pid1;

                                                            }
                                                        }
                                                        else if (yn == "2")
                                                        {
                                                            Console.Clear();
                                                            goto ar1;
                                                        }
                                                        else
                                                        {
                                                            throw new InvalidChoiceException("Invalid choice! Try again.");
                                                        }
                                                    }
                                                    catch (InvalidChoiceException ex)
                                                    {
                                                        WriteCentered(ex.Message);
                                                        goto ss;
                                                    }

                                                }
                                                catch (InvalidChoiceException ex)
                                                {
                                                    WriteCentered(ex.Message);
                                                }
                                                break;

   
                                            case "2":
                                                if (productService.GetProductsByCategory((Category)Enum.Parse(typeof(Category), sc4)).Count() == 0)
                                                {
                                                    WriteCentered("No products found.");
                                                    goto ar1;
                                                }

                                                WriteCentered($"Delete Product from {(Category)Enum.Parse(typeof(Category), sc4)}");

                                                //productService.DisplayProductsWithDetails(pname, (Category)Enum.Parse(typeof(Category), sc4));
                                                productService.DisplayProductNamesWithCount((Category)Enum.Parse(typeof(Category), sc4));

                                                WriteCentered("");
                                            pname:
                                                string pname1 = ReadCentered("Enter product name to delete (0 to back) : ");
                                                try
                                                {
                                                    if (pname1 == "0")
                                                    {
                                                        goto ar1;
                                                    }
                                                    if (string.IsNullOrEmpty(pname1))
                                                    {
                                                        throw new InvalidProductDetailsException("Invalid product name! Try again.");

                                                    } 
                                                    
                                                }
                                                catch (InvalidProductDetailsException ex)
                                                {
                                                    WriteCentered(ex.Message);
                                                    goto pname;
                                                }
                                                WriteCentered("");
                                                try
                                                {
                                                    if (!productService.DeleteProduct(pname1))
                                                    {
                                                        throw new ProductNotFoundException("Product not found.");
                                                    }
                                                }
                                                catch (ProductNotFoundException ex)
                                                {
                                                    WriteCentered(ex.Message);
                                                    goto pname;

                                                } 
                                                break;

                                            default:
                                                Console.Clear();
                                                throw new InvalidChoiceException("Invalid choice! Try again.");

                                        }
                                    }
                                    catch(InvalidChoiceException ex)
                                    {
                                        WriteCentered(ex.Message);
                                        goto ar1;
                                    }



                                }


                                //productService.DisplayProducts(productService.GetProductsByCategory((Category)Enum.Parse(typeof(Category), sc4)));
                            }
                            catch (InvalidChoiceException ex)
                            {
                                WriteCentered(ex.Message);
                                goto adminmenu;
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


