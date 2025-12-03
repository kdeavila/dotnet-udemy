using CsBases.Fundamentals;

class Program
{
  static void Main()
  {
    // Instancias dos objetos de Product y Service Product
    var laptop = new Product("Laptop", 1200);
    WriteLine(laptop.GetDescription());

    var soporte = new ServiceProduct("Soporte Técnico", 300, 2);
    WriteLine(soporte.GetDescription());

    // Instanciar el la clase Product usando el patrón adaptador de una Classe a un DTOs
    var product = new Product("Mouse gamer", 300);
    var productDto = ProductAdaptader.ToDto(product);
    WriteLine($"{productDto.Name} - {productDto.Price:C} - Código: {productDto.Code}");
  }
}