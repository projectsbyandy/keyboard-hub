namespace VendorApi.Models.Auth;

public record LoginResponse(LoginOutcome LoginOutcome, string Value)
{
    public LoginOutcome Outcome { get; init; } = LoginOutcome;
}