using Backend.Context;
using Backend.DTOs.InventoryDTO;
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
        var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        
        product.SKU = SKU;
        await _context.SaveChangesAsync();
    }

    public async Task<Dictionary<string, object?>?> GetProducyBySKU(string SKU)
    {
        var result = await _context.Product
            .AsNoTracking() // read-only for performance
            .Where(p => p.SKU == SKU)
            .Select(p => new Dictionary<string, object?>
            {
                { "Id", p.Id },
                { "Name", p.productName },
                { "Price", p.price },
                { "Stock", p.stock },
                { "Cost", p.cost },
                { "CategoryId", p.category.Id },
                { "CategoryName", p.category.categoryName }
            })
            .FirstOrDefaultAsync();

        return result;

    }


}