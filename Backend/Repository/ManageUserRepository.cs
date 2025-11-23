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
    public async Task<string> AddUserRepository(AddUserDTO user)
    {
        var mapUser = new Users()
        {
            UserName = user.username,
            Email = user.email,
            FirstName = user.firstName,
            MiddleName = user.middleName,
            SurName = user.surName
        };
        var result = await manager.CreateAsync(mapUser, user.password);
        
    }
}