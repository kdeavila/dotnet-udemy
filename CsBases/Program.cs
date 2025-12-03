using System.Threading.Tasks;
using CsBases.Fundamentals;

class Program
{
  static async Task Main()
  {
    // Instancias dos objetos de Product y Service Product
    var laptop = new Product("Laptop", 1200);
    WriteLine(laptop.GetDescription());

    var soporte = new ServiceProduct("Soporte Técnico", 300, 2);
    WriteLine(soporte.GetDescription());

    // Instanciar el la clase Product usando el patrón adaptador de una Classe a un DTOs
    var product = new Product("Mouse gamer", 300);
    var productDto = ProductAdapter.ToDto(product);
    WriteLine($"{productDto.Name} - {productDto.Price:C} - Código: {productDto.Code}");

    // Inyección de dependencias
    // Instanciar el servicio que se va a a inyectar
    LabelService labelService = new LabelService();

    // Instanciar la clase ProductManager que requiere la inyección del servicio
    var manager = new ProductManager(labelService);

    var monitor = new Product("Monitor", 100);
    var installation = new ServiceProduct("Instalación de monitor", 20, 30);

    // Usar el método PrintLabel de ProductManager para imprimir
    // LabelService está siendo inyectado en ProductManager  
    manager.PrintLabel(monitor);
    manager.PrintLabel(installation);

    // Métodos asíncronos con Async/Await
    var firstProduct = await new ProductRepository().getProduct(1);
    firstProduct.Description = "Esta es la descripción del primer producto";

    // Hacemos uso del Attributo para los propiedas con [UpperCase] en Product.cs 
    AttributeProcessor.ApplyUpperCase(firstProduct);
    WriteLine($"{firstProduct.Name} - {firstProduct.Price} - {firstProduct.Description}");
  }
}