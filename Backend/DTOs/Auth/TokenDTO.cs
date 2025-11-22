namespace Backend.DTOs.Auth;

public class TokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}