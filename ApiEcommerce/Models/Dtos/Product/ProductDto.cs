namespace ApiEcommerce.Models.Dtos.Product;

public class ProductDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public string ImageUrl { get; set; } = string.Empty;
  public string SKU { get; set; } = string.Empty;
  public int Stock { get; set; }
  public DateTime CreationDate { get; set; } = DateTime.Now;
  public DateTime? UpdateDate { get; set; } = null;

  // Relación con el modelo Category
  public int CategoryId { get; set; }

  // Propiedad nombre de la categoría
  public string CategoryName { get; set; } = string.Empty;
}
