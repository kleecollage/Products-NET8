using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Repository;

public class MoviePhotoRepository(ApplicationDbContext context): IMoviePhotoRepository<PeliculaFoto>
{
  private readonly ApplicationDbContext _context = context;

    public IEnumerable<PeliculaFoto> GetAll()
    {
        return [.. _context.Set<PeliculaFoto>().OrderByDescending(m => m.Id)];
    }

    public PeliculaFoto GetById(int id)
    {
        return _context.Set<PeliculaFoto>().Find(id);
    }

    public IEnumerable<PeliculaFoto> GetPhotosByMovie(int id)
    {
        return [.. _context.Set<PeliculaFoto>().Where(p => p.PeliculaId == id)];
    }

    public void Add(PeliculaFoto entity)
    {
        _context.Set<PeliculaFoto>().Add(entity);
        _context.SaveChanges();
    }

    public void Update(PeliculaFoto entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var data = _context.Set<PeliculaFoto>().Find(id);
        if (data != null)
        {
          _context.Set<PeliculaFoto>().Remove(data);
          _context.SaveChanges();
        }
    }

}