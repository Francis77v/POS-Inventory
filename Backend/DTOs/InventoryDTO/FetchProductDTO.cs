namespace Backend.DTOs.InventoryDTO;

public class FetchProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CategoryDTO category { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public decimal? Cost { get; set; }
}