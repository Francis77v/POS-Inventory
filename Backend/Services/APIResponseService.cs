using Backend.DTOs;

namespace Backend.Services;

public static class APIResponseService
{
    public static APIResponseDTO<T> SuccessResponseService<T>(string message, int status_code, T data)
    {
        return new APIResponseDTO<T>()
        {
            success = true,
            StatusCode = status_code,
            message = message,
            data = data
        };
    }
    
    public static APIResponseDTO<T> ErrorResponseService<T>(string message, int status_code, IEnumerable<string> errors = null)
    {
        return new APIResponseDTO<T>()
        {
            success = false,
            StatusCode = status_code,
            message = message,
            data = default(T),
            Errors = errors?.ToList() ?? new List<string>()
        };
    }
}