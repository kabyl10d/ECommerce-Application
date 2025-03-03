public interface IUserService
{
    void Register(string username, string mailid, string password, string phone,string role,string upiid,int upipin);
    User Login(string username, string password);
}
