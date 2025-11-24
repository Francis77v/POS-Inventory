using Backend.Model;
using Microsoft.AspNetCore.Identity;

namespace Backend.Repository;
using Backend.DTOs.ManageUserDTO;
public class ManageUserRepository
{
    private readonly UserManager<Users> manager;

    public ManageUserRepository(UserManager<Users> _manager)
    {
        manager = _manager;
    }
    
    public async Task<IdentityResult> AddUserRepository(Users user)
    {
        var exists = await CheckUserIfExist(user.UserName, user.Email);
        if (exists)
            return IdentityResult.Failed(new IdentityError { Description = "User exists" });

        return await manager.CreateAsync(user, user.PasswordHash);
    }
    
    public async Task<bool> CheckUserIfExist(string username, string email)
    {
        var checkUserName = await manager.FindByNameAsync(username);
        var checkEmail = await manager.FindByEmailAsync(email);
        if (checkUserName != null && checkEmail != null)
        {
            return true;
        }

        return false;
    }
}