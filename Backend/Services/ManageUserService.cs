using Backend.DTOs;
using Backend.DTOs.ManageUserDTO;
using Backend.Model;
using Backend.Repository;

namespace Backend.Services;

public class ManageUserService
{
    private readonly ManageUserRepository repository;

    public ManageUserService(ManageUserRepository _repository)
    {
        repository = _repository;
    }

    public async Task<APIResponseDTO<string>> AddUserService(AddUserDTO user)
    {
        if (user.password != user.confirmPassword)
        {
            return APIResponseService.Error<string>(
                "Retry Password",
                403,
                new List<string> { "Password mismatched." }
            );
        }

        try
        {
            var mapUser = new Users()
            {
                UserName = user.username,
                Email = user.email,
                FirstName = user.firstName,
                MiddleName = user.middleName,
                SurName = user.surName,
                PasswordHash = user.password
            };

            var result = await repository.AddUserRepository(mapUser);
            if (!result.Succeeded)
            {
                return APIResponseService.Conflict<string>(
                    "User already exists!",
                    new List<string> { result.Errors.First().Description }
                );
            }
            
            return APIResponseService.Success(
                data: "User successfully created.",
                message: "User successfully created."
            );
        }
        catch (Exception e)
        {
            return APIResponseService.Error<string>(
                "Error creating user.",
                500,
                new List<string> { e.Message }
            );
        }
    }

}