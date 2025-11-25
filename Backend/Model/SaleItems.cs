namespace Backend.Model;

public class SaleItems
{
    public int Id { get; set; }
    
    //fk
    public int saleId { get; set; }
    public Sales sale { get; set; }
    public int productId { get; set; }
    public Products product { get; set; }
    
    public int quantity { get; set; }
    public decimal price { get; set; }
    public decimal subtotal { get; set; }
}