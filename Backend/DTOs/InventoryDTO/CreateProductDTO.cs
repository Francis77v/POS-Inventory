using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.InventoryDTO;

public class CreateProductDTO
{
    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }  // FK to Categories

    [Required(ErrorMessage = "Price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be zero or positive.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; set; } = 0; // initial stock, optional default 0

    [Range(0, double.MaxValue, ErrorMessage = "Cost must be zero or positive.")]
    public decimal? Cost { get; set; } // optional
}