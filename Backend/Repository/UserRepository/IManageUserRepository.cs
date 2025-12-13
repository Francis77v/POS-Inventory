using Backend.DTOs.ManageUserDTO;
using Backend.Model;
using Microsoft.AspNetCore.Identity;

namespace Backend.Repository.UserRepository;

public interface IManageUserRepository
{
    Task<IdentityResult> AddUserRepository(Users user);

    Task<bool> CheckUserIfExist(string username, string email);

    Task<List<GetUserDTO>> GetUserRepository();

    Task UpdateUserRepository(Users user);

    Task UpdatePasswordRepository(Users user, string newPw);

    Task DeleteUserRepository(Users user);

    Task<Users?> FetchUserById(string id);
}