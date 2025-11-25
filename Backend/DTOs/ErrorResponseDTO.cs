namespace Backend.DTOs;

public class ErrorResponseDTO<T>
{
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
}