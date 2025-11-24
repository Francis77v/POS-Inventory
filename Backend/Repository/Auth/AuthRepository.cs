using Backend.Services.Authentication;
using Backend.DTOs.Auth;
using Backend.Model;
using Microsoft.AspNetCore.Identity;

namespace Backend.Repository.Auth;

public class AuthRepository
{
    private readonly UserManager<Users> manager;
    public AuthRepository(UserManager<Users> userManager)
    {
        manager = userManager;
    }
    public async Task<TokenDTO?> CheckUser(LoginDTO user)
    {
        var result = await manager.FindByNameAsync(user.username);
        if (result != null && await manager.CheckPasswordAsync(result, user.password))
        {
            var token = await service.GenerateJwtToken(result);
            return token;
        }

        return null;
    }
    
    
}