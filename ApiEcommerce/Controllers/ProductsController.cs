using AutoMapper;
using ApiEcommerce.Models;
using Microsoft.AspNetCore.Mvc;
using ApiEcommerce.Models.Dtos.Product;
using ApiEcommerce.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace ApiEcommerce.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "Admin")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiController]
public class ProductsController : ControllerBase
{
   private readonly IProductRepository _productRepository;
   private readonly ICategoryRepository _categoryRepository;
   private readonly IMapper _mapper;

   public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
   {
      _productRepository = productRepository;
      _categoryRepository = categoryRepository;
      _mapper = mapper;
   }

   [AllowAnonymous]
   [HttpGet]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]

   public IActionResult GetProducts()
   {
      var products = _productRepository.GetProducts();
      var productsDto = _mapper.Map<List<ProductDto>>(products);

      return Ok(productsDto);
   }

   [AllowAnonymous]
   [HttpGet("{id:int}", Name = "GetProduct")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]

   public IActionResult GetProduct(int id)
   {
      var product = _productRepository.GetProduct(id);
      if (product == null) return NotFound($"El producto con id {id} no existe");

      var productDto = _mapper.Map<ProductDto>(product);
      return Ok(productDto);
   }

   [HttpPost]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status401Unauthorized)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto)
   {
      if (createProductDto == null) return BadRequest(ModelState);

      if (_productRepository.ProductExists(createProductDto.Name))
      {
         ModelState.AddModelError("CustomError", "El producto ya existe");
         return BadRequest(ModelState);
      }

      if (!_categoryRepository.CategoryExists(createProductDto.CategoryId))
      {
         ModelState.AddModelError("CustomError", $"La categoría con id {createProductDto.CategoryId} no existe");
         return BadRequest(ModelState);
      }

      var product = _mapper.Map<Product>(createProductDto);

      if (!_productRepository.CreateProduct(product))
      {
         ModelState.AddModelError("CustomError", $"Algo salió mal guardando el registro {product.Name}");
         return StatusCode(500, ModelState);
      }

      var productCreated = _productRepository.GetProduct(product.Id);
      var productDto = _mapper.Map<ProductDto>(productCreated);

      return CreatedAtRoute("GetProduct", new { id = product.Id }, productDto);
   }

   [AllowAnonymous]
   [HttpGet("category/{categoryId:int}", Name = "GetProductsForCategory")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]

   public IActionResult GetProductsForCategory(int categoryId)
   {
      if (!_categoryRepository.CategoryExists(categoryId)) return NotFound($"La categoría con id {categoryId} no existe");

      var products = _productRepository.GetProductsForCategory(categoryId);
      var productsDto = _mapper.Map<ICollection<ProductDto>>(products);
      return Ok(productsDto);
   }

   [AllowAnonymous]
   [HttpGet("search/{search}", Name = "SearchProducts")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   public IActionResult SearchProducts(string search)
   {
      var products = _productRepository.SearchProducts(search);
      if (products.Count == 0)
      {
         return NotFound($"Productos no encontrados con el término '{search}'");
      }

      var productsDto = _mapper.Map<ICollection<ProductDto>>(products);
      return Ok(productsDto);
   }

   [HttpPatch("buy/{name}/{quantity:int}", Name = "BuyProduct")]
   public IActionResult BuyProduct(string name, int quantity)
   {
      if (string.IsNullOrEmpty(name) || quantity <= 0) return BadRequest("Nombre o cantidad de producto inválidos");

      var product = _productRepository.ProductExists(name);
      if (!product) return BadRequest($"El producto con nombre '{name}' no existe");

      if (!_productRepository.BuyProduct(name, quantity)) return BadRequest($"No se pudo comprar '{name}' o la cantidad de {quantity} sobrepasa el stock actual");

      var units = quantity == 1 ? "unidad" : "unidades";
      return Ok($"Compró {quantity} {units} del producto '{name}'");
   }


   [HttpPut("{id:int}", Name = "UpdateProduct")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status401Unauthorized)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status500InternalServerError)]
   public IActionResult UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
   {
      if (updateProductDto == null) return BadRequest(ModelState);

      if (!_productRepository.ProductExists(id))
      {
         ModelState.AddModelError("CustomError", $"El producto con id {id} no existe");
         return BadRequest(ModelState);
      }

      if (!_categoryRepository.CategoryExists(updateProductDto.CategoryId))
      {
         ModelState.AddModelError("CustomError", $"La categoría con id {updateProductDto.CategoryId} no existe");
         return BadRequest(ModelState);
      }

      var product = _mapper.Map<Product>(updateProductDto);
      product.Id = id;

      if (!_productRepository.UpdateProduct(product))
      {
         ModelState.AddModelError("CustomError", $"Algo salió mal actualizando el registro {product.Name}");
         return StatusCode(500, ModelState);
      }

      return NoContent();
   }

   [HttpDelete("{id:int}", Name = "DeleteProduct")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status403Forbidden)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]

   public IActionResult DeleteProduct(int id)
   {
      var product = _productRepository.GetProduct(id);
      if (product == null) return NotFound($"El producto con id {id} no existe");

      if (!_productRepository.DeleteProduct(product))
      {
         ModelState.AddModelError("CustomError", $"Algo salió mal eliminando el registro {product.Name}");
         return StatusCode(500, ModelState);
      }

      return NoContent();
   }
}