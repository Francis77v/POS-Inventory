using Backend.DTOs;
using Backend.DTOs.Auth;
using Backend.Repository.Auth;
namespace Backend.Services.Authentication;

public class AuthServices
{
    private readonly AuthRepository repository;
    
    public AuthServices(AuthRepository _repository)
    {
        repository = _repository;
    }

    public async Task<APIResponseDTO<TokenDTO>> CheckUserService(LoginDTO user)
    {
        try
        {
            var result = await repository.CheckUser(user);
            if (result != null)
            {
                return new APIResponseDTO<TokenDTO>()
                {
                    success = true,
                    StatusCode = 200,
                    message = "Welcome User",
                    data = new TokenDTO()
                    {
                        AccessToken = result.AccessToken,
                        RefreshToken = result.RefreshToken
                    }
                };
            }
            return new APIResponseDTO<TokenDTO>()
            {
                success = false,
                StatusCode = 401,
                message = "User not found",
                Errors = new List<string>()
                {
                    "Invalid User"
                }
            };
        }
        catch (Exception e)
        {
            return new APIResponseDTO<TokenDTO>()
            {
                success = false,
                StatusCode = 500,
                message = "Database Error",
                Errors = new List<string>()
                {
                    e.Message
                }
            };
        }
    }
}