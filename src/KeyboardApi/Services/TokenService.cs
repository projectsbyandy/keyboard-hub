using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KeyboardApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace KeyboardApi.Services;

public class TokenService : ITokenService
{
    private readonly TimeSpan _expiryDuration = new TimeSpan(0, 30, 0);
    
    public string BuildToken(string key, string issuer, User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };
 
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
            expires: DateTime.Now.Add(_expiryDuration), signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}