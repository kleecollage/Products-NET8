using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Repository;

public class MovieRepository(ApplicationDbContext context): IMovieRepository<Pelicula>
{
  private readonly ApplicationDbContext _context = context;

  public IEnumerable<Pelicula> GetAll()
  {
      return [.. _context.Set<Pelicula>().OrderByDescending(p => p.Id)];
  }
  public Pelicula GetById(int id)
  {
      return _context.Set<Pelicula>().Find(id);
  }

  public void Add(Pelicula entity)
  {
    _context.Set<Pelicula>().Add(entity);
    _context.SaveChanges();
  }

  public void Update(Pelicula entity)
  {
    _context.Entry(entity).State = EntityState.Modified;
    _context.SaveChanges();
  }

  public void Delete(int id)
  {
    var movie = _context.Set<Pelicula>().Find(id);
    if (movie != null)
    {
      _context.Set<Pelicula>().Remove(movie);
      _context.SaveChanges();
    }
  }

    public IEnumerable<Pelicula> GetAllByTheme(int theme_id)
    {
        return [.. _context.Set<Pelicula>().Where(m => m.TematicaId == theme_id)];
    }

    public IEnumerable<Pelicula> GetAllSearcher(string search)
    {
        return [.. _context.Set<Pelicula>().Where(p => p.Nombre.Contains(search))];
    }

    public IEnumerable<Pelicula> GetPagedMovies(int page, int pageSize)
    {
        return [.. _context.Set<Pelicula>()
          .OrderByDescending(m => m.Id)
          .Skip((page -1) * pageSize)
          .Take(pageSize)
          .Include(x => x.Tematica)
          ];
    }

    public IEnumerable<Pelicula> GetPagedMoviesByTheme(int theme_id, int page, int pageSize)
    {
        return [.. _context.Set<Pelicula>()
          .Where(p => p.TematicaId == theme_id)
          .OrderByDescending(m => m.Id)
          .Skip((page -1) * pageSize)
          .Take(pageSize)
          .Include(x => x.Tematica)];
    }

    public IEnumerable<Pelicula> GetPagedMovieSearcher(string search, int page, int pageSize)
    {
        return [.. _context.Set<Pelicula>()
          .Where(p => p.Nombre.Contains(search))
          .OrderByDescending(p => p.Id)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .Include(x => x.Tematica)];
    }

}