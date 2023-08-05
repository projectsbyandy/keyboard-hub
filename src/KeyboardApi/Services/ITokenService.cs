using KeyboardApi.Models;
using KeyboardApi.Models.Config;

namespace KeyboardApi.Services;

public interface ITokenService
{
    public string BuildToken(JwtConfig jwtConfig, User user);
}