using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
  public DbSet<Categoria> Categorias { get; set; }
  public DbSet<Producto> Productos { get; set; }
  public DbSet<ProductosFotos> ProductosFotos { get; set; }
  public DbSet<Tematica> Tematicas { get; set; }
  public DbSet<Pelicula> Peliculas { get; set; }
  public DbSet<PeliculaFoto> PeliculasFotos { get; set; }
}
