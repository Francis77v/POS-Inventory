using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.DTOs.Auth;
using Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Repository.Auth;

public class AuthRepository
{
    private readonly UserManager<Users> manager;
    private readonly IConfiguration config;
    
    public AuthRepository(UserManager<Users> userManager, IConfiguration _config)
    {
        manager = userManager;
        config = _config;
    }
    public async Task<TokenDTO?> CheckUser(LoginDTO user)
    {
        var result = await manager.FindByNameAsync(user.username);
        if (result != null && await manager.CheckPasswordAsync(result, user.password))
        {
            var token = await GenerateJwtToken(result);
            return token;
        }

        return null;
    }
    
    private async Task<TokenDTO> GenerateJwtToken(Users user)
    {
        var userRoles = await manager.GetRolesAsync(user);

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }
            .Concat(userRoles.Select(role => new Claim(ClaimTypes.Role, role)))
            .ToList();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(config["Jwt:ExpireMinutes"])),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return new TokenDTO()
        {
            AccessToken = tokenString,
            RefreshToken = refreshToken
        };
    }
}