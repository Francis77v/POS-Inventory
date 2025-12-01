using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.InventoryDTO;

public class SkuDTO
{
    [Required(ErrorMessage = "Product SKU is required")]
    public string SKU { get; set; }
}