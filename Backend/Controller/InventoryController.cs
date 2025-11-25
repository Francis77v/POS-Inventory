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
    }
}
