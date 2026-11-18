using System.ComponentModel.DataAnnotations;


public class Category
{
    public int CategoryId { get; set; }
    [Required]
    [MaxLength(80)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string Description { get; set; }
    public List<Product> Products { get; set; } = new();
}
