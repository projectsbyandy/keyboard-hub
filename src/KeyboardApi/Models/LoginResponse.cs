#nullable disable
using System.Text.Json.Serialization;

namespace KeyboardApi.Models;

public class LoginResponse
{
    [JsonPropertyName("Token")]
    public string Token { get; init; }
}