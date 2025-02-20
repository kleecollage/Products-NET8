using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Repository;

public class TematicaRepository(ApplicationDbContext context) : ITematicaRepository<Tematica>
{
  private readonly ApplicationDbContext _context = context;

  public IEnumerable<Tematica> GetAll()
  {
      return [.. _context.Set<Tematica>().OrderByDescending(t => t.Id)];
  }

  public Tematica GetById(int id)
  {
      return _context.Set<Tematica>().Find(id);
  }
  public void Add(Tematica entity)
  {
      _context.Set<Tematica>().Add(entity);
      _context.SaveChanges();
  }

  public void Update(Tematica entity)
  {
      _context.Entry(entity).State = EntityState.Modified;
      _context.SaveChanges();
  }
  public void Delete(int id)
  {
      var tematica = _context.Set<Tematica>().Find(id);
      if (tematica != null)
      {
        _context.Set<Tematica>().Remove(tematica);
        _context.SaveChanges();
      }
  }

}