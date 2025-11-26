namespace Backend.Model;

public class Products
{
    public int Id { get; set; }
    
    public string productName { get; set; }
    public string? SKU { get; set; }
    
    public int categoryId { get; set; }
    public Category category { get; set; }
    
    public decimal price { get; set; }
    public int stock { get; set; }
    public decimal? cost { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<SaleItems> saleItems { get; } = new List<SaleItems>(); 
    public ICollection<StockMovements> stocks { get; } = new List<StockMovements>(); 
}