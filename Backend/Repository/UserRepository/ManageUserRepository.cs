using Backend.DTOs.ManageUserDTO;
using Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository.UserRepository;

public class ManageUserRepository : IManageUserRepository
{
    private readonly UserManager<Users> _manager;

    public ManageUserRepository(UserManager<Users> manager)
    {
        _manager = manager;
    }
    
    public async Task<IdentityResult> AddUserRepository(Users user)
    {
        var exists = await CheckUserIfExist(user.UserName, user.Email);
        if (exists)
            return IdentityResult.Failed(new IdentityError { Description = "User exists" });

        return await _manager.CreateAsync(user, user.PasswordHash);
    }
    
    public async Task<bool> CheckUserIfExist(string username, string email)
    {
        var checkUserName = await _manager.FindByNameAsync(username);
        var checkEmail = await _manager.FindByEmailAsync(email);

        // Return true if **either username or email exists**
        return checkUserName != null || checkEmail != null;
    }

    public async Task<List<GetUserDTO>> GetUserRepository()
    {
        return await _manager.Users
            .AsTracking()
            .Select(u => new GetUserDTO()
            {
                Id = u.Id,
                firstName = u.FirstName,
                middleName = u.MiddleName,
                surName = u.SurName,
                username = u.UserName,
            })
            .ToListAsync();
    }

    public async Task UpdateUserRepository(Users user)
    {
        await _manager.UpdateAsync(user);
    }

    public async Task UpdatePasswordRepository(Users user, string newPw)
    {
        var token = await _manager.GeneratePasswordResetTokenAsync(user);
        await _manager.ResetPasswordAsync(user, token, newPw);
    }

    public async Task DeleteUserRepository(Users user)
    {
        await _manager.DeleteAsync(user);
    }
    
    //helper methods
    public async Task<Users?> FetchUserById(string id)
    {
        return await _manager.FindByIdAsync(id);
    }

}