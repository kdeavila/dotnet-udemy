namespace ApiEcommerce.Models.Dtos.Product;

public class CreateProductDto
{
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public string? ImageUrl { get; set; }
  public IFormFile? Image { get; set; }
  public string SKU { get; set; } = string.Empty;
  public int Stock { get; set; }

  // Relación con el modelo Category
  public int CategoryId { get; set; }
}
