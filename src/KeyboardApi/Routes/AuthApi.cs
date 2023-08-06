using KeyboardApi.Models;
using KeyboardApi.Models.Config;
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

            if (locatedUser.Password == user.Password)
            {
                var token = tokenService.BuildToken(jwtConfig, locatedUser);

                return Results.Ok(token);   
            }

            return Results.Unauthorized();
        });
    }
}