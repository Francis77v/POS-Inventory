using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Backend.DTOs;
using Backend.DTOs.Auth;
namespace Backend.Services.Authentication;

public class AuthServices
{
    private readonly UserManager<Users> manager;
    private readonly IConfiguration config;
    
    public AuthServices(UserManager<Users> userManager, IConfiguration configuration)
    {
        manager = userManager;
        config = configuration;
    }

    public async Task<APIResponseDTO<TokenDTO>> CheckUserService(LoginDTO user)
    {
        try
        {
            var result = await manager.FindByNameAsync(user.username);
            if (result != null && await manager.CheckPasswordAsync(result, user.password))
            {
                var token = await GenerateJwtToken(result);
                return new APIResponseDTO<TokenDTO>()
                {
                    success = true,
                    StatusCode = 200,
                    message = "Welcome User",
                    data = new TokenDTO()
                    {
                        AccessToken = token.AccessToken,
                        RefreshToken = token.RefreshToken
                    }
                };
            }

            return new APIResponseDTO<TokenDTO>()
            {
                success = false,
                StatusCode = 401,
                message = "User not found",
                Errors = new List<string>()
                {
                    "Invalid User"
                }
            };
        }
        catch (Exception e)
        {
            return new APIResponseDTO<TokenDTO>()
            {
                success = false,
                StatusCode = 500,
                message = "Database Error",
                Errors = new List<string>()
                {
                    e.Message
                }
            };
        }
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