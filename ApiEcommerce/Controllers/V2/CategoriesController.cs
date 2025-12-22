using ApiEcommerce.Constants;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos.Category;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers.V2;

// Controlador para manejar operaciones CRUD de Categorías en su versión 2
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
[ApiController]
[Authorize(Roles = "Admin")]
public class CategoriesController : ControllerBase
{
   private readonly ICategoryRepository _categoryRepository;
   private readonly IMapper _mapper;

   public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
   {
      _categoryRepository = categoryRepository;
      _mapper = mapper;
   }

   [AllowAnonymous]
   [HttpGet]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status200OK)]

   public IActionResult GetCategoriesOrderById()
   {
      var categories = _categoryRepository.GetCategories().OrderBy(c => c.Id);
      var categoriesDto = new List<CategoryDto>();

      foreach (var category in categories)
      {
         categoriesDto.Add(_mapper.Map<CategoryDto>(category));
      }
      return Ok(categoriesDto);
   }


   [ResponseCache(CacheProfileName = CacheProfiles.Default10)]
   [AllowAnonymous]
   [HttpGet("{id:int}", Name = "GetCategory")]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [ProducesResponseType(StatusCodes.Status200OK)]

   public IActionResult GetCategory(int id)
   {
      Console.WriteLine($"Fetching data... {id} {DateTime.Now}");
      var category = _categoryRepository.GetCategory(id);
      Console.WriteLine($"Fetching data... {id}");

      if (category == null) return NotFound($"La categoría con id {id} no existe");

      var categoryDto = _mapper.Map<CategoryDto>(category);
      return Ok(categoryDto);
   }

   [HttpPost]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status401Unauthorized)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
   {
      if (createCategoryDto == null) return BadRequest(ModelState);

      if (_categoryRepository.CategoryExists(createCategoryDto.Name))
      {
         ModelState.AddModelError("CustomError", "La categoría ya existe");
         return BadRequest(ModelState);
      }

      var category = _mapper.Map<Category>(createCategoryDto);

      if (!_categoryRepository.CreateCategory(category))
      {
         ModelState.AddModelError("CustomError", $"Algo salió mal guardando el registro {category.Name}");
         return StatusCode(500, ModelState);
      }

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
