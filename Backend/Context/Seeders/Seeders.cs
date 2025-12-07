using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace Backend.Context.Seeders;

public static class Seeders
{
    public static void RoleSeeder(this ModelBuilder modelBuilder)
    {
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
    }   
}