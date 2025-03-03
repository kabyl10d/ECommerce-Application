using System.Text.RegularExpressions;
class UserService : IUserService
{
    private static void WriteCentered(string text)
    {
        int windowWidth = Console.WindowWidth;
        int textLength = text.Length;
        int spaces = (windowWidth - textLength) / 2;
        Console.WriteLine(new string(' ', spaces) + text);
    }
     
    //public string userFilePath = "C:\\Users\\10decoders\\source\\repos\\ECommerceAppFinal\\ECommerceAppFinal\\Data\\user.txt";
    public static List<User> users = new List<User>();

    public bool ValidPhoneNumber(string phone)
    {
        string pattern = @"^[6-9]\d{9}$";
        Regex r = new Regex(pattern);
        return r.IsMatch(phone);
    }
    public bool ValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    public void Register(string username, string mailid, string password,string phone, string role,string upiid, int upipin)
    {

   
        int userId = users.Count + 1;
        User newUser = role.ToLower() == "merchant"
            ? new Merchant(userId, mailid, username, password, phone, upiid, upipin)
            : new Customer(userId, mailid, username, password, phone, upiid, upipin);
         
        users.Add(newUser);


        // FileHelper.WriteToFile(userFilePath, $"{userId}|{mailid}|{username}|{password}|{role}");

        WriteCentered("Registration successful!");
    }

    public User Login(string username, string password)
    {
        User user = users.Find(u => (u.Username == username || u.Mailid == username) && u.Password == password);
        if (user == null)
        {
            WriteCentered("Invalid username or password!");
            return null;
        }

        WriteCentered($"Welcome, {user.Username}! - {user.Role}");
        return user;
    }
}