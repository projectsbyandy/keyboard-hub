using VendorApi.Models.Auth;
using VendorApi.Models.Config;
using VendorApi.Repository.Auth;

namespace VendorApi.Services.Internal;

internal class AuthService(IUserRepository userRepository, ITokenService tokenService, JwtConfig jwtConfig) : IAuthService
{
    public LoginResponse Login(User user)
    {
        var locatedUser = userRepository.GetUser(user.Email);
        
        if (locatedUser is null)
        {
            return new LoginResponse(LoginOutcome.NotFound, user.Email);
        }

        return locatedUser.Password.Equals(user.Password) 
            ? new LoginResponse(LoginOutcome.Ok, tokenService.BuildToken(jwtConfig, locatedUser))
            : new LoginResponse(LoginOutcome.InvalidPassword, user.Email);
    }
}