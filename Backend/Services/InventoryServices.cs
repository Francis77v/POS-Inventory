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
            if(product == null) return APIResponseService.NotFound<FetchProductDTO>(message: "Product not found.");
            return APIResponseService.Success(data: product);
        }
        catch (UnauthorizedAccessException e)
        {
            return APIResponseService.Unauthorized<FetchProductDTO>(
                errors: new List<string>() {e.Message}
                );
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
        catch (KeyNotFoundException e)
        {
            return APIResponseService.NotFound<string>(message: "Product not found", errors: new List<string>() { e.Message });
        }
        catch (UnauthorizedAccessException e)
        {
            return APIResponseService.Unauthorized<string>(errors: new List<string>() {e.Message});
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
                return APIResponseService.NotFound<List<FetchProductDTO>>(
                    message: "No products found."
                );
            }

            return APIResponseService.Success(data: result);

        }
        catch (UnauthorizedAccessException e)
        {
            return APIResponseService.Unauthorized<List<FetchProductDTO>>(errors: new List<string>() { e.Message });
        }

        catch (Exception e)
        {
            return APIResponseService.Error<List<FetchProductDTO>>(
                errors: new List<string> { e.Message }
            );
        }
    }

    public async Task<APIResponseDTO<string>> UpdateProductService(int id, UpdateProductDTO p)
    {
        try
        {
            var product = await _repository.FIndProductById(id);
            if (product == null)
            {
                return APIResponseService.NotFound<string>(message: "Product does not exist");
            }

            if (p.productName != null) product.productName = p.productName;
            if (p.categoryId.HasValue) product.categoryId = p.categoryId.Value;
            if (p.cost.HasValue) product.cost = p.cost;
            if (p.price.HasValue) product.price = p.price.Value;
            if (p.stock.HasValue) product.stock = p.stock.Value;
            var category = await _categoryRepository.FetchCategoryRepository(product.categoryId);
            product.SKU = $"{category.Substring(0, 3).ToUpper()}-{product.productName.ToUpper()}-{product.Id}";

            await _repository.UpdateProductDetailsRepository(product);
            return APIResponseService.Success(data: "Product Updated Successfully.");

        }
        catch (UnauthorizedAccessException e)
        {
            return APIResponseService.Unauthorized<string>();
        }
        catch (Exception e)
        {
            return APIResponseService.Error<string>(message: "An unexpected error occurred while updating the product.",
                statusCode: 500,
                errors: new List<string> { e.Message });
        }
        
    }

    
}