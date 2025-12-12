using Backend.DTOs;
using Backend.DTOs.ManageUserDTO;
using Backend.Model;
using Backend.Repository;
using Backend.Repository.UserRepository;

namespace Backend.Services.UserService;

public class ManageUserService
{
    private readonly ManageUserRepository _repository;

    public ManageUserService(ManageUserRepository repository)
    {
        _repository = repository;
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

            var result = await _repository.AddUserRepository(mapUser);
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

    public async Task<APIResponseDTO<List<GetUserDTO>>> GetUserService()
    {
        try
        {
            var result = await _repository.GetUserRepository();
            if (result == null) return APIResponseService.NotFound<List<GetUserDTO>>();
            return APIResponseService.Success(data: result);
        }
        catch (UnauthorizedAccessException e)
        {
            return APIResponseService.Unauthorized<List<GetUserDTO>>(errors: new List<string>() {e.Message});
        }
        catch (Exception e)
        {
            return APIResponseService.Error<List<GetUserDTO>>(message: "Internal Server Error", statusCode: 500, errors: new List<string>(){e.Message});
        }
    }

    public async Task<APIResponseDTO<string>> UpdateUserService(string id, UpdateUserDTO user)
    {
        try
        {
            var result = await _repository.FetchUserById(id);

            if (result == null) return APIResponseService.NotFound<string>(message: "User not found");

            if (user.firstName != null) result.FirstName = user.firstName;
            if (user.middleName != null) result.MiddleName = user.middleName;
            if (user.surName != null) result.SurName = user.surName;
            if (user.userName != null) result.UserName = user.userName;

            await _repository.UpdateUserRepository(result);
            return APIResponseService.Success<string>(message: "User Updated.");
        }
        catch (UnauthorizedAccessException e)
        {
            return APIResponseService.Unauthorized<string>();
        }
        catch (Exception e)
        {
            return APIResponseService.Error<string>(message: "Internal Server Error", statusCode: 500);
        }
    }

    public async Task<APIResponseDTO<string>> UpdatePasswordService(string id, PasswordDTO password)
    {
        try
        {
            var user = await _repository.FetchUserById(id);
            if (user == null) return APIResponseService.NotFound<string>(message: "User not found");
            await _repository.UpdatePasswordRepository(user, password.password);
            return APIResponseService.Success<string>(message: "User password resetted");
        }
        catch (UnauthorizedAccessException e)
        {
            return APIResponseService.Unauthorized<string>(errors: new List<string>() {e.Message});
        }
        catch (Exception e)
        {
            return APIResponseService.Error<string>(
                statusCode: 500, message: "Internal Server Error", errors: new List<string>() {e.Message}
                );
        }
    }
    
    public async Task<APIResponseDTO<string>> DeleteUserService(string id)
    {
        try
        {
            var user = await _repository.FetchUserById(id);
            if (user == null) return APIResponseService.NotFound<string>(message: "User not found");
            await _repository.DeleteUserRepository(user);
            return APIResponseService.Success<string>(message: "User deleted");
        }
        catch (UnauthorizedAccessException e)
        {
            return APIResponseService.Unauthorized<string>(errors: new List<string>(){e.Message});
        }
        catch (Exception e)
        {
            return APIResponseService.Error<string>(
                statusCode: 500, message:"Internal Server Error", errors: new List<string>() {e.Message}
                );
        }
    }
    

}