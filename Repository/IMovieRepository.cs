namespace Web.Repository;

public interface IMovieRepository<T>
{
  T GetById(int id);
  IEnumerable<T> GetAll();
  IEnumerable<T> GetAllByTheme(int theme_id);
  IEnumerable<T> GetPagedMovies(int page, int pageSize);
  IEnumerable<T> GetPagedMoviesByTheme(int theme_id, int page, int pageSize);
  IEnumerable<T> GetAllSearcher(string search);
  IEnumerable<T> GetPagedMovieSearcher(string search, int page, int pageSize);
  void Add(T entity);
  void Update(T entity);
  void Delete(int id);
}