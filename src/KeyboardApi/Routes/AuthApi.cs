using Keyboard.Common.Models;
using Keyboard.Common.Models.Config;
using KeyboardApi.Repository.Auth;
using KeyboardApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace KeyboardApi.Routes;

public static class AuthApi
{
    public static void AddAuthRoutes(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", [AllowAnonymous] (User user, ITokenService tokenService, IUserRepository userRepository, JwtConfig jwtConfig) => {

            var locatedUser = userRepository.GetUser(user.Email);
            if (locatedUser is null)
            {
                return Results.NotFound(user.Email);
            }

            if (locatedUser.Password.Equals(user.Password))
            {
                var token = tokenService.BuildToken(jwtConfig, locatedUser);

                return Results.Ok(new LoginResponse()
                {
                    Token = token
                });   
            }

            return Results.Unauthorized();
        });
    }
}