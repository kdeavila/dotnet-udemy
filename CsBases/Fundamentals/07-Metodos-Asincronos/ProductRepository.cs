namespace CsBases.Fundamentals;

public class ProductRepository
{
  // Podría obtener los datos de una base de datos,
  // una llamada a una API externa
  // o podría ser de un archivo
  public async Task<Product> getProduct(int id)
  {
    WriteLine("Buscando el producto...");

    await Task.Delay(2000);
    return new Product("Producto simulado", 150);
  }
}
