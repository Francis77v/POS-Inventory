namespace Backend.DTOs.ManageUserDTO;

using System.ComponentModel.DataAnnotations;

public class AddUserDTO
{
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
    [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
    public string username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email format is invalid.")]
    public string email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required.")]
    [Compare("password", ErrorMessage = "Password and Confirm Password do not match.")]
    public string confirmPassword { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(100)]
    public string firstName { get; set; }

    [MaxLength(100)]
    public string middleName { get; set; }

    [Required(ErrorMessage = "Surname is required.")]
    [MaxLength(100)]
    public string surName { get; set; }

    // [Required(ErrorMessage = "Role is required.")]
    // public Roles role { get; set; }
}
