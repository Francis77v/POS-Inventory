using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Backend.Model;
namespace Backend.Context.Seeders;

public static class Seeders
{
    public static void DataSeeder(this ModelBuilder modelBuilder)
    {
        var hasher = new PasswordHasher<Users>();
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new IdentityRole
            {
                Id = "2",
                Name = "Cashier",
                NormalizedName = "CASHIER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        );

        var user = new Users
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "Darvin",
            MiddleName = "Oma",
            SurName = "Palma",
            Email = "darvin@email.com",
            NormalizedEmail = "DARVIN@EMAIL.COM",
            UserName = "darvin123",
            NormalizedUserName = "DARVIN123"
        };

        user.PasswordHash = hasher.HashPassword(user, "Darvin123!");

        modelBuilder.Entity<Users>().HasData(user);

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "1",
                UserId = user.Id
            }
        );
    }   
}