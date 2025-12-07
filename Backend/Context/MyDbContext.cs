using Backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Backend.Context.Seeders;
namespace Backend.Context;

public partial class MyDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Users> User { get; set; }
    public DbSet<Products> Product { get; set; }
    public DbSet<Category> category { get; set; }
    public DbSet<Sales> sales { get; set; }
    public DbSet<SaleItems> saleItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Role seeder
        Seeders.Seeders.RoleSeeder(modelBuilder);
        
        //Category 1:M Products
        modelBuilder.Entity<Category>()
            .HasMany(e => e.product)
            .WithOne(e => e.category)
            .HasForeignKey(e => e.categoryId)
            .IsRequired();
        
        //Products 1:M SaleItems
        modelBuilder.Entity<Products>()
            .HasMany(e => e.saleItems)
            .WithOne(e => e.product)
            .HasForeignKey(e => e.productId)
            .IsRequired();
        
        //Products 1:M StockMovements
        modelBuilder.Entity<Products>()
            .HasMany(e => e.stocks)
            .WithOne(e => e.product)
            .HasForeignKey(e => e.productId)
            .IsRequired();
        
        //Sales M:1 User
        modelBuilder.Entity<Users>()
            .HasMany(e => e.Sales)
            .WithOne(e => e.user)
            .HasForeignKey(e => e.cashier)
            .IsRequired();
        
        //Sales 1:M SaleItems
        modelBuilder.Entity<Sales>()
            .HasMany(e => e.SalesItems)
            .WithOne(e => e.sale)
            .HasForeignKey(e => e.saleId)
            .IsRequired();


    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
