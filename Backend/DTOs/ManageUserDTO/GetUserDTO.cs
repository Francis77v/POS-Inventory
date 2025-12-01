namespace Backend.DTOs.ManageUserDTO;

public class GetUserDTO
{
    public string Id { get; set; }
    public string firstName { get; set; }
    public string? middleName { get; set; }
    public string surName { get; set; }
    public string username { get; set; }
    // public string role { get; set; }
}