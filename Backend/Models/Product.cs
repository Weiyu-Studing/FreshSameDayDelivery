using System.ComponentModel.DataAnnotations;

public class Product
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }

    [Range(0.01, 9999, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Range(0, 9999)]
    public int Stock { get; set; }

    public string ImageUrl { get; set; }
    public string Unit { get; set; }

    [Required]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
