using ApiEcommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);
   }

   public DbSet<Category> Categories { get; set; } = null!;
   public DbSet<Product> Products { get; set; } = null!;
   public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
}
