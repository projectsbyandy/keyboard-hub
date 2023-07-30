using KeyboardApi.Models;

namespace KeyboardApi.Services;

public interface ITokenService
{
    public string BuildToken(string key, string issuer, User user);
}