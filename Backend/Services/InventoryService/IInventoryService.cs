using Backend.DTOs;
using Backend.DTOs.InventoryDTO;

namespace Backend.Services.InventoryService;

public interface IInventoryService
{
    Task<APIResponseDTO<string>> CreateProductService(CreateProductDTO product);

    Task<APIResponseDTO<FetchProductDTO>> GetProductService(SkuDTO sku);

    Task<APIResponseDTO<string>> DeleteProductService(SkuDTO skuDto);

    Task<APIResponseDTO<List<FetchProductDTO>>> FetchAllProductService();

    Task<APIResponseDTO<string>> UpdateProductService(int id, UpdateProductDTO p);
}