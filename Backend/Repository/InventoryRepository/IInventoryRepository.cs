using Backend.DTOs.InventoryDTO;
using Backend.Model;

namespace Backend.Repository.InventoryRepository;

public interface IInventoryRepository
{
    Task<bool> CheckProductIfExist(string product);

    Task AddProductRepository(Products product);

    Task<int> CheckLastProductName(string productName);

    Task UpdateSKUofLastProduct(string SKU, int id);

    Task<FetchProductDTO?> GetProducyBySKU(Products SKU);

    Task DeleteProductRepository(Products sku);

    Task<List<FetchProductDTO>> FetchAllProductsRepository();

    Task<Products?> FIndProductById(int Id);

    Task UpdateProductDetailsRepository(Products products);
}