using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.ManageUserDTO;

public class PasswordDTO
{
    [Required(ErrorMessage = "Password is required.")]
    public string password { get; set; }
}