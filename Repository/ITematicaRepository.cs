using Web.Models;

namespace Web.Repository;

public interface ITematicaRepository<T>
{
  T GetById(int id);
  IEnumerable<Tematica> GetAll();
  void Add(T entity);
  void Update(T entity);
  void Delete(int id);
}