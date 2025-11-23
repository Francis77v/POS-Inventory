namespace Backend.DTOs.ManageUserDTO;

public class AddUserDTO
{
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string firstName { get; set; }
    public string middleName { get; set; }
    public string surName { get; set; }
    public Roles role { get; set; }
}