namespace Backend.Model;

public class Sales
{
    //pk
    public int Id { get; set; }
    
    //Fk
    public string cashier { get; set; }
    public Users user { get; set; }
    
    public decimal totalAmount { get; set; }
    public decimal paymentMethod { get; set; }
    public decimal change { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<SaleItems> SalesItems { get; } = new List<SaleItems>(); 
}