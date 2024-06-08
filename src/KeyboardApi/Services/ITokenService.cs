using Keyboard.Common.Models;
using Keyboard.Common.Models.Config;

namespace KeyboardApi.Services;

public interface ITokenService
{
    public string BuildToken(JwtConfig jwtConfig, User user);
}