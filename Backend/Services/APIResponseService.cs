using Backend.DTOs;

namespace Backend.Services;

public static class APIResponseService
{
    public static APIResponseDTO<T> Success<T>(string message = "Operation successful", T? data = default)
    {
        return new APIResponseDTO<T>
        {
            success = true,
            StatusCode = 200,
            message = message,
            data = data
        };
    }
    
    public static APIResponseDTO<T> Error<T>(string message = "An error occurred", int statusCode = 400, List<string>? errors = null)
    {
        return new APIResponseDTO<T>
        {
            success = false,
            StatusCode = statusCode,
            message = message,
            Errors = errors
        };
    }
    
    public static APIResponseDTO<T> NotFound<T>(string message = "Resource not found", List<string>? errors = null)
    {
        return Error<T>(message, 404, errors);
    }

    public static APIResponseDTO<T> Conflict<T>(string message = "Conflict occurred", List<string>? errors = null)
    {
        return Error<T>(message, 409, errors);
    }
    
    public static APIResponseDTO<T> Unauthorized<T>(string message = "Unauthorized", List<string>? errors = null)
    {
        return Error<T>(message, 401, errors);
    }
}
