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

    public async Task<APIResponseDTO<string>> CreateProductService(CreateProductDTO product)
    {
        if (await _repository.CheckProductIfExist(product.Name))
        {
            return APIResponseService.Conflict<string>(
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
            var cat= await _categoryRepository.FetchCategoryRepository(product.CategoryId);
            var prodId = await _repository.CheckLastProductName(product.Name);
            string Category = cat.Length >= 3 ? cat.Substring(0, 3) : cat;
            //SKU Format
            string skuFormat = $"{Category.ToUpper()}-{product.Name.ToUpper()}-{prodId + 1}";
            await _repository.UpdateSKUofLastProduct(skuFormat, prodId);
            
            return APIResponseService.Success(data: "Product Successfully added to Inventory.");
        }
        catch (DbUpdateException dbEx)
        {
            return APIResponseService.Error<string>(
                message: "Failed to add product due to database error.",
                statusCode: 500,
                errors: new List<string> { dbEx.InnerException?.Message ?? dbEx.Message  }
            );
        }
        catch (ArgumentNullException argEx)
        {
            return APIResponseService.Error<string>(
                message: "Product data cannot be null.",
                statusCode: 400,
                errors: new List<string> { argEx.Message }
            );
        }
        catch (Exception ex)
        {
            return APIResponseService.Error<string>(
                message: "An unexpected error occurred while adding the product.",
                statusCode: 500,
                errors: new List<string> { ex.Message }
            );
        }
        
    }

    public async Task<APIResponseDTO<FetchProductDTO>> GetProductService(SkuDTO sku)
    {
        var mapSku = new Products()
        {
            SKU = sku.SKU
        };
        try
        {
            var product = await _repository.GetProducyBySKU(mapSku);
            return APIResponseService.Success<FetchProductDTO>(data: product);
        }
        catch (Exception e)
        {
            return APIResponseService.Error<FetchProductDTO>(
                message: "An unexpected error occurred while adding the product.",
                statusCode: 500,
                errors: new List<string> { e.Message }
            );
        }
        
    }
    
    public async Task<APIResponseDTO<string>> DeleteProductService(SkuDTO skuDto)
    {
        try
        {
            var mapSku = new Products() { SKU = skuDto.SKU };
            await _repository.DeleteProductRepository(mapSku);

            return APIResponseService.Success(data: "Product Deleted.");
        }
        catch (Exception e)
        {
            return APIResponseService.Error<string>(message: "An unexpected error occurred while deleting the product.",
                statusCode: 500,
                errors: new List<string> { e.Message });
        }
    }

    public async Task<APIResponseDTO<List<FetchProductDTO>>> FetchAllProductService()
    {
        try
        {
            var result = await _repository.FetchAllProductsRepository();
            if (!result.Any())
            {
                return APIResponseService.Success<List<FetchProductDTO>>(
                    message: "No products found.",
                    data: new List<FetchProductDTO>()
                );
            }
            return APIResponseService.Success(data: result);

        }
        catch (Exception e)
        {
            return APIResponseService.Error<List<FetchProductDTO>>(
                errors: new List<string> { e.Message }
            );
        }
    }
}