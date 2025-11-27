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

    public async Task<FetchProductDTO?> GetProducyBySKU(Products SKU)
    {
        return await _context.Product
            .AsNoTracking()
            .Include(p => p.category)
            .Where(p => p.SKU == SKU.SKU)
            .Select(p => new FetchProductDTO
            {
                Id = p.Id,
                SKU = p.SKU,
                Name = p.productName,
                Price = p.price,
                Stock = p.stock,
                Cost = p.cost,
                category = new CategoryDTO
                {
                    categoryId = p.category.Id,
                    categoryName = p.category.categoryName
                }
            })
            .FirstOrDefaultAsync();
    }

    public async Task DeleteProductRepository(Products sku)
    {
        var productDelete = await _context.Product.Where(p => p.SKU == sku.SKU).FirstOrDefaultAsync();
        if (productDelete == null)
        {
            throw new Exception($"Product with SKU:{sku} Not Found");
        }
        
        _context.Product.Remove(productDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<List<FetchProductDTO>> FetchAllProductsRepository()
    {
        return await _context.Product
            .AsNoTracking()
            .Select(p => new FetchProductDTO()
            {
                Id = p.Id,
                SKU = p.SKU,
                Name = p.productName,
                Price = p.price,
                Stock = p.stock,
                Cost = p.cost,
                category = new CategoryDTO
                {
                    categoryId = p.category.Id,
                    categoryName = p.category.categoryName
                }
            }).ToListAsync();
    }

    public async Task<Products?> FIndProductById(int Id)
    {
        var product = await _context.Product
            .Where(p => p.Id == Id)
            .FirstOrDefaultAsync();
        return product;
    }

    public async Task UpdateProductDetailsRepository(int Id, Products products)
    {
        var product = await FIndProductById(Id);
        if (product == null)
        {
            throw new Exception("Product does not exist");
        }
        
        //update record
        product.productName = products.productName;
        product.categoryId = products.categoryId;
        product.stock = products.stock;
        product.cost = products.cost;
        product.price = products.price;

         _context.Product.Update(product);
         await _context.SaveChangesAsync();

    }
    


}