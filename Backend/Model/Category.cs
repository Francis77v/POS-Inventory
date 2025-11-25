namespace Backend.Model;

public class Category
{
    public int Id { get; set; }
    public string categoryName { get; set; }

    public ICollection<Products> product { get; } = new List<Products>(); 
}