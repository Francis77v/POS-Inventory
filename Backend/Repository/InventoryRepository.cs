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
}