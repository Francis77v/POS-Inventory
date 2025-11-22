namespace Backend.DTOs;

public class APIResponseDTO<T>
{
    public bool success { get; set; }
    public int StatusCode { get; set; }
    public string message { get; set; }
    public T? data { get; set; }
    public List<string>? Errors { get; set; }
}