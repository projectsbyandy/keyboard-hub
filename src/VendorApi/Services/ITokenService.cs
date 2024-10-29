using VendorApi.Models.Auth;
using VendorApi.Models.Config;

namespace VendorApi.Services;

public interface ITokenService
{
    public string BuildToken(JwtConfig jwtConfig, User user);
}