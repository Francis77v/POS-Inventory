using Microsoft.AspNetCore.Identity;

namespace Backend.Model;

public class Users : IdentityUser
{
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string SurName { get; set; }
}