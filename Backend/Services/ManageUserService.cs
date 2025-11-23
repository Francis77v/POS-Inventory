using Backend.DTOs;
using Backend.DTOs.ManageUserDTO;
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
            return new APIResponseDTO<string>()
            {
                success = false,
                StatusCode = 404,
                message = "Retry Password",
                Errors = new List<string>()
                {
                    "Password mismatched."
                }
            };
        }

        try
        {
            var result = await repository.AddUserRepository(user);
            return new APIResponseDTO<string>()
            {
                success = true,
                StatusCode = 200,
                message = "User successfully created."
            };

        }
        catch (Exception e)
        {
            return new APIResponseDTO<string>()
            {
                success = false,
                StatusCode = 500,
                message = "Error creating user.",
                Errors = new List<string>()
                {
                    e.Message
                }
            };
        }
    }
}