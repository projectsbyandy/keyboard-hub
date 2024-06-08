using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Keyboard.Common.Models;
using Keyboard.Common.Models.Config;
using Microsoft.IdentityModel.Tokens;

namespace KeyboardApi.Services;

public class TokenService : ITokenService
{
    public string BuildToken(JwtConfig jwtConfig, User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var jwtSecurityToken = new JwtSecurityToken(
            expires: DateTime.Now.AddMinutes(30),
            claims: claims,
            signingCredentials: credentials,
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience);

       return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}