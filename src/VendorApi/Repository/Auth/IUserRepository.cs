using VendorApi.Models.Auth;

namespace VendorApi.Repository.Auth;

public interface IUserRepository
{
    public User? GetUser(string name);
}