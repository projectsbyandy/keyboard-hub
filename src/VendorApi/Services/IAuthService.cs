using VendorApi.Models.Auth;

namespace VendorApi.Services;

public interface IAuthService
{
    public LoginResponse Login(User user);
}