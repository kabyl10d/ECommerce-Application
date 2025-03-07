public interface IUserService
{
    void Register(string username, string mailid, string password, string phone,string role);
    User Login(string username, string password);
}
