using ApiEcommerce.Constants;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos.Category;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers;

// Controlador para manejar operaciones CRUD de Categorías
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
[EnableCors(PolicyNames.AllowSpecificOrigin)]
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
  // AllowAnonymous permite a cualquier usuario acceder al endpoint sin autenticarse o estar autorizado
  [AllowAnonymous]
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status200OK)]

  // Podemos usar el decorador EnableCors("nombreDeLaPolitíca")
  // Añade CORS a métodos o controladores específicos
  // Se recomienda usar constantes para evitar errores de tipado
  // [EnableCors("AllowSpecificOrigin")]

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

  [AllowAnonymous]
  [HttpGet("{id:int}", Name = "GetCategory")]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status200OK)]

  public IActionResult GetCategory(int id)
  {
    var category = _categoryRepository.GetCategory(id);
    if (category == null) return NotFound($"La categoría con id {id} no existe");

    var categoryDto = _mapper.Map<CategoryDto>(category);
    return Ok(categoryDto);
  }

  // Atributos que definen los posibles códigos de respuesta HTTP para esta acción
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
  {
    // Validación básica: si el DTO es nulo, retorna BadRequest
    if (createCategoryDto == null) return BadRequest(ModelState);

    // Chequea si la categoría ya existe para evitar duplicados
    if (_categoryRepository.CategoryExists(createCategoryDto.Name))
    {
      ModelState.AddModelError("CustomError", "La categoría ya existe");
      return BadRequest(ModelState);
    }

    // Mapea el DTO a la entidad Category usando AutoMapper
    var category = _mapper.Map<Category>(createCategoryDto);

    // Intenta crear la categoría; si falla, retorna error 500
    if (!_categoryRepository.CreateCategory(category))
    {
      ModelState.AddModelError("CustomError", $"Algo salió mal guardando el registro {category.Name}");
      return StatusCode(500, ModelState);
    }

    // Retorna 201 Created con la ruta para obtener la categoría recién creada
    return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
  }

  [HttpPatch("{id:int}", Name = "UpdateCategory")]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]

  public IActionResult UpdateCategory(int id, [FromBody] CreateCategoryDto updateCategoryDto)
  {
    if (!_categoryRepository.CategoryExists(id)) return NotFound($"La categoría con id {id} no existe");
    if (updateCategoryDto == null) return BadRequest(ModelState);

    if (_categoryRepository.CategoryExists(updateCategoryDto.Name))
    {
      ModelState.AddModelError("CustomError", "La categoría ya existe");
      return BadRequest(ModelState);
    }

    var category = _mapper.Map<Category>(updateCategoryDto);
    category.Id = id;

    if (!_categoryRepository.UpdateCategory(category))
    {
      ModelState.AddModelError("CustomError", $"Algo salió mal al actualizar el registro {category.Name}");
      return StatusCode(500, ModelState);
    }

    return NoContent();
  }

  [HttpDelete("{id:int}", Name = "DeteleCategory")]
  public IActionResult DeteleCategory(int id)
  {
    if (!_categoryRepository.CategoryExists(id)) return NotFound($"La categoría con id {id} no existe");

    var category = _categoryRepository.GetCategory(id);
    if (category == null) return NotFound($"La categoría con id {id} no existe");

    if (!_categoryRepository.DeleteCategory(category))
    {
      ModelState.AddModelError("CustomError", $"Algo salió mal al eliminar el registro {category.Name}");
      return StatusCode(500, ModelState);
    }

    return NoContent();
  }
}
