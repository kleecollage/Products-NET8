namespace Web.Repository;

public interface IMoviePhotoRepository<T>
{
  T GetById(int id);
  IEnumerable<T> GetAll();
  IEnumerable<T> GetPhotosByMovie(int id);
  void Add(T entity);
  void Update(T entity);
  void Delete(int id);
}