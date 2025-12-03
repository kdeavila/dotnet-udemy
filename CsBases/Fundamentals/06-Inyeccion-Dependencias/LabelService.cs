namespace CsBases.Fundamentals;

public class LabelService : ILabelService
{
  public string GenerateLabel(Product product)
  {
    // Utiliza GetType para saber que tipo de dato es
    // Utiliza GetHashCode para generar un código según el producto
    return $"{product.Name} - Precio: {product.Price} - Código: {product.GetType().Name}-{product.GetHashCode()}";
  }
}
