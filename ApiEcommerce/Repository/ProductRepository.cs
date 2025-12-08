using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Repository;

public class ProductRepository : IProductRepository
{
  private readonly ApplicationDbContext _db;
  private readonly ICategoryRepository _categoryRepository;

  public ProductRepository(ApplicationDbContext db, ICategoryRepository categoryRepository)
  {
    _db = db;
    _categoryRepository = categoryRepository;
  }

  public bool BuyProduct(string name, int quantity)
  {
    if (string.IsNullOrWhiteSpace(name) || quantity <= 0) return false;

    var product = _db.Products.FirstOrDefault(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
    if (product == null || quantity > product.Stock) return false;

    product.Stock -= quantity;

    _db.Products.Update(product);
    return Save();
  }

  public bool CreateProduct(Product product)
  {
    if (product == null) return false;

    product.CreationDate = DateTime.Now;
    product.UpdateDate = DateTime.Now;

    _db.Products.Add(product);
    return Save();
  }

  public bool UpdateProduct(Product product)
  {
    if (product == null) return false;
    product.UpdateDate = DateTime.Now;

    _db.Products.Update(product);
    return Save();
  }

  public bool DeleteProduct(Product product)
  {
    if (product == null) return false;

    _db.Products.Remove(product);
    return Save();
  }

  public Product? GetProduct(int id)
  {
    if (id <= 0) return null;
    return _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
  }

  public ICollection<Product> GetProducts()
  {
    return _db.Products.Include(p => p.Category).OrderBy(p => p.Name).ToList();
  }

  public ICollection<Product> GetProductsForCategory(int categoryId)
  {
    if (categoryId <= 0) return new List<Product>();

    return _db.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId).OrderBy(p => p.Name).ToList();
  }

  public bool ProductExists(int id)
  {
    if (id <= 0) return false;
    return _db.Products.Any(p => p.Id == id);
  }

  public bool ProductExists(string name)
  {
    if (string.IsNullOrWhiteSpace(name)) return false;
    return _db.Products.Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
  }

  public ICollection<Product> SearchProducts(string search)
  {
    IQueryable<Product> query = _db.Products;
    var normalizedSearch = search.ToLower().Trim();

    if (!string.IsNullOrEmpty(search))
    {
      query = query
      .Include(p => p.Category)
      .Where(p => p.Name.ToLower().Trim().Contains(normalizedSearch) ||
                  p.Description.ToLower().Trim().Contains(normalizedSearch)
      );
    }

    return query.OrderBy(p => p.Name).ToList();
  }

  public bool Save()
  {
    return _db.SaveChanges() >= 0;
  }

}
