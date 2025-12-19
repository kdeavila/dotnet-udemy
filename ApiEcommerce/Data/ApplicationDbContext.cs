using ApiEcommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

   public DbSet<Category> Categories { get; set; } = null!;
   public DbSet<Product> Products { get; set; } = null!;
   public DbSet<User> Users { get; set; } = null!;
}
