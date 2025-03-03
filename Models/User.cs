public abstract class User
{
    public int UserId { get; private set; }
    public string Username { get; private set; }

    public string Mailid { get; private set; }
    public string Password { get; private set; }
    public string Role { get; private set; }

    public string Upiid;

    public int Upipin;

    public string Phone;
    protected User(int userId, string mailid, string username, string password, string phone,string role,string upiid, int upipin)
    {
        UserId = userId;
        Mailid = mailid;
        Username = username;
        Password = password;
        Phone = phone;
        Role = role;
        Upipin = upipin;
        Upiid = upiid;
    }

    public abstract void DisplayDetails();
}

//public class Admin : User
//{
//    public Admin(int userId, string mailid, string username, string password,string phone)
//        : base(userId, mailid, username, password, phone ,"Admin") { }

//    public override void DisplayDetails()
//    {
//        Console.WriteLine($"Admin: {Username} \nMail Address: {Mailid} ");
//    }
//}

public class Customer : User
{
    public Customer(int userId, string mailid,string username, string password, string phone, string upiid, int upipin)
        : base(userId, mailid,username, password, phone , "Customer",upiid,upipin) { }

    public override void DisplayDetails()
    {
        Console.WriteLine($"Customer: {Username} \nMail Address: {Mailid}\nPhone number : {Phone}");
    }
}


public class Merchant : User
{
    public List<Product> products = new List<Product>();
    public Merchant(int userId, string mailid, string username, string password, string phone,string upiid,int upipin)
        : base(userId, mailid, username, password, phone, "Merchant",upiid, upipin) { }

    public override void DisplayDetails()
    {
        Console.WriteLine($"Merchant: {Username} \nMail Address: {Mailid} \nPhone number : {Phone}");
    }


}