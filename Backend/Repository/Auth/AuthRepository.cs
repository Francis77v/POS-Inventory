
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
    public async Task<Users?> CheckUserRepository(Users user)
    {
        var result = await manager.FindByNameAsync(user.UserName);
        if (result != null && await manager.CheckPasswordAsync(result, user.PasswordHash))
        {
            return result;
        }
        return null;
    }
    
    
}