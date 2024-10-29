using VendorApi.Models.Auth;
using VendorApi.Services;
using ILogger = Serilog.ILogger;

namespace VendorApi.Endpoints;

public static class AuthenticationApi
{
    public static void AddAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/authenticate");
        
        group.MapPost("login", Login);
    }

    private static IResult Login(User user, IAuthService authService, ILogger logger)
    {
        var response = authService.Login(user);

        if (response.Outcome is LoginOutcome.Ok)
        {
            return Results.Content(response.Value);
        }

        logger.Warning("Unable to login using {Email}, due to {LoginOutcome}", user.Email, response.LoginOutcome);
        return Results.Unauthorized();
    }
}