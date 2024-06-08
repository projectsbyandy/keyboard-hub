#nullable disable
using System.Text.Json.Serialization;

namespace Keyboard.Common.Models;

public class LoginResponse
{
    [JsonPropertyName("Token")]
    public string Token { get; init; }
}