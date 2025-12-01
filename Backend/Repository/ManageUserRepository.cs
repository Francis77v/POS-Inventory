using Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;
using Backend.DTOs.ManageUserDTO;
public class ManageUserRepository
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

}