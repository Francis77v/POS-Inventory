using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.DTOs;
using Backend.DTOs.Auth;
using Backend.Model;
using Backend.Repository.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services.Authentication;

public class AuthServices
{
    private readonly AuthRepository repository;
    private readonly UserManager<Users> manager;
    private readonly IConfiguration config;
    
    public AuthServices(AuthRepository _repository, UserManager<Users> _manager, IConfiguration _config)
    {
        repository = _repository;
        manager = _manager;
        config = _config;
    }

    public async Task<APIResponseDTO<TokenDTO>> CheckUserService(LoginDTO user)
    {
        var userMapping = new Users()
        {
            UserName = user.username,
            PasswordHash = user.password
        };

        try
        {
            var result = await repository.CheckUserRepository(userMapping);

            if (result != null)
            {
                var token = await GenerateJwtToken(result);

                return APIResponseService.Success(
                    data: new TokenDTO
                    {
                        AccessToken = token.AccessToken,
                        RefreshToken = token.RefreshToken
                    },
                    message: "Welcome User"
                );
            }

            return APIResponseService.Unauthorized<TokenDTO>(
                "User not found",
                new List<string> { "Invalid User" }
            );
        }
        catch (Exception e)
        {
            return APIResponseService.Error<TokenDTO>(
                "Database Error",
                500,
                new List<string> { e.Message }
            );
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