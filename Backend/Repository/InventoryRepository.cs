using Backend.Context;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class InventoryRepository
{
    private readonly MyDbContext _context;

    public InventoryRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckProductIfExist(string product)
    {
        var result = await _context.Product.AnyAsync(p => p.productName == product);
        if (result) return true;
        return false;
    }

    public async Task AddProductRepository(Products product)
    {
        await _context.Product.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CheckLastProductName(string productName)
    {
        var result = await _context.Product
            .Where(p => p.productName == productName)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task UpdateSKUofLastProduct(string SKU, int id)
    {
        var product = new Products { Id = id, SKU = SKU };
        _context.Product.Attach(product);
        _context.Entry(product).Property(p => p.SKU).IsModified = true;
        await _context.SaveChangesAsync();
    }

}