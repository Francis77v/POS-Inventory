namespace Backend.Model;

public class Sales
{
    //pk
    public int Id { get; set; }
    
    //Fk
    public string cashier { get; set; }
    public Users user { get; set; }
    
    public float totalAmount { get; set; }
    public float paymentMethod { get; set; }
    public float change { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<SaleItems> SalesItems { get; } = new List<SaleItems>(); 
}