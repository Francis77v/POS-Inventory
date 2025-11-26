using Backend.Context;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repository;

public class CategoryRepository
{
    private readonly MyDbContext _context;

    public CategoryRepository(MyDbContext context)
    {
        _context = context;
    }

    public async Task<string> FetchCategoryRepository(int id)
    {
        var result = await _context.category
            .Where(c => c.Id == id)
            .Select(c => c.categoryName)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task CreateCategoryRepository(string categoryName)
    {
        
    }
}