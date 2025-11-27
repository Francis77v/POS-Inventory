using Backend.DTOs.InventoryDTO;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryServices _services;
        public InventoryController(InventoryServices services)
        {
            _services = services;
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> AddProduct(CreateProductDTO product)
        {
            var result = await _services.CreateProductService(product);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetProduct(SkuDTO SKU)
        {
            var result = await _services.GetProductService(SKU);
            return StatusCode(result.StatusCode, result);
        }
        
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProduct(SkuDTO SKU)
        {
            var result = await _services.DeleteProductService(SKU);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("display")]
        public async Task<IActionResult> FetchAllProduct()
        {
            var result = await _services.FetchAllProductService();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("update/{Id}")]
        public async Task<IActionResult> UpdateProduct(int Id, UpdateProductDTO product)
        {
            var result = await _services.UpdateProductService(Id, product);
            return StatusCode(result.StatusCode, result);
        }
    }
}
