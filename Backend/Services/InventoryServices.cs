using Backend.DTOs;
using Backend.DTOs.InventoryDTO;
using Backend.Repository;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class InventoryServices
{
    private readonly InventoryRepository _repository;
    private readonly CategoryRepository _categoryRepository;

    public InventoryServices(InventoryRepository repository, CategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<APIResponseDTO<SuccessResponseDTO>> CreateProductService(CreateProductDTO product)
    {
        if (await _repository.CheckProductIfExist(product.Name))
        {
            return APIResponseService.Conflict<SuccessResponseDTO>(
                "Product already exists",
                new List<string> { $"Product with name '{product.Name}' already exists." }
            );
        }
        var mapProduct = new Products()
        {
            productName = product.Name,
            SKU = null,
            categoryId = product.CategoryId,
            price = product.Price,
            stock = product.Stock,
            cost = product.Cost
        };

        try
        {
            await _repository.AddProductRepository(mapProduct);
            var cat= await _categoryRepository.FetchCategory(product.CategoryId);
            var prodId = await _repository.CheckLastProductName(product.Name);
            string Category = cat.Length >= 3 ? cat.Substring(0, 3) : cat;
            //SKU Format
            string skuFormat = $"{Category.ToUpper()}-{product.Name.ToUpper()}-{prodId + 1}";
            await _repository.UpdateSKUofLastProduct(skuFormat, prodId);
            
            return APIResponseService.Success(data: new SuccessResponseDTO()
            {
                Data = "Product Successfully added to Inventory."
            });
        }
        catch (DbUpdateException dbEx)
        {
            return APIResponseService.Error<SuccessResponseDTO>(
                message: "Failed to add product due to database error.",
                statusCode: 500,
                errors: new List<string> { dbEx.InnerException?.Message ?? dbEx.Message  }
            );
        }
        catch (ArgumentNullException argEx)
        {
            return APIResponseService.Error<SuccessResponseDTO>(
                message: "Product data cannot be null.",
                statusCode: 400,
                errors: new List<string> { argEx.Message }
            );
        }
        catch (Exception ex)
        {
            return APIResponseService.Error<SuccessResponseDTO>(
                message: "An unexpected error occurred while adding the product.",
                statusCode: 500,
                errors: new List<string> { ex.Message }
            );
        }
        
    }
}