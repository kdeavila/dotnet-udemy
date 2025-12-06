using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers;

// Controlador para manejar operaciones CRUD de Categorías
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
  private readonly ICategoryRepository _categoryRepository;
  private readonly IMapper _mapper;

  // Inyección de dependecias: repositorio de categorías y el mapper de AutoMapper
  public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
  {
    _categoryRepository = categoryRepository;
    _mapper = mapper;
  }

  // Método que obtiene todas las categorías disponibles
  // Usamos el decorador/atributo HttpGet para que el endpoint responda a un HTTP GET 
  // Usamos ProducesResponseType para alimentar correctamente la documentación de la API con OpenAPI y Swagger
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status200OK)]

  public IActionResult GetCategories()
  {
    // Obtener todas las categorías desde el repositorio.
    var categories = _categoryRepository.GetCategories();
    
    var categoriesDto = new List<CategoryDto>();

    // Mapear cada categoría a su DTO correspondiente.
    foreach (var category in categories)
    {
      // Utilizamos el mapper de AutoMapper para convertir cada categoría en su DTO correspondiente.
      categoriesDto.Add(_mapper.Map<CategoryDto>(category));
    }

    // Retornar la lista de DTOs con un código de estado 200 OK.
    // El método Ok devuelve un objeto IActionResult con un código de estado HTTP 200 OK.
    return Ok(categoriesDto);
  }
}
