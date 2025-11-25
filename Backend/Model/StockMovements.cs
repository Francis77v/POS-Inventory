using Backend.Model.enums;
using Type = Backend.Model.enums.Type;

namespace Backend.Model;

public class StockMovements
{
    public int Id { get; set; }
    
    //fk
    public int productId { get; set; }
    public Products product { get; set; }
    
    
    public Type type { get; set; }
    public int quantity { get; set; }
    public string reference { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}