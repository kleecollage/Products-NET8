using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data;

public class ApplicationDbContext: DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

  public DbSet<Categoria> Categorias { get; set; }
  public DbSet<Producto> Productos { get; set; }
  public DbSet<ProductoFoto> ProductoFotos { get; set; }
}
