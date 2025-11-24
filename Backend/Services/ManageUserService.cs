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

    public async Task<APIResponseDTO<SuccessResponseDTO>> AddUserService(AddUserDTO user)
    {
        if (user.password != user.confirmPassword)
        {
            return new APIResponseDTO<SuccessResponseDTO>()
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
            var mapUser = new Users()
            {
                UserName = user.username,
                Email = user.email,
                FirstName = user.firstName,
                MiddleName = user.middleName,
                SurName = user.surName,
                PasswordHash = user.password
            };
            await repository.AddUserRepository(mapUser);
            var checkUser = await repository.CheckUserIfExist(user.username, user.email);
            if (!checkUser)
            {
                return new APIResponseDTO<SuccessResponseDTO>()
                {
                    success = false,
                    StatusCode = 404,
                    message = "Create different user credentials",
                    Errors = new List<string>()
                    {
                        "User already exists in database."
                    }
                };
            }

            return new APIResponseDTO<SuccessResponseDTO>()
            {
                success = true,
                StatusCode = 200,
                message = "User successfully created."
            };

        }
        catch (Exception e)
        {
            return new APIResponseDTO<SuccessResponseDTO>()
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