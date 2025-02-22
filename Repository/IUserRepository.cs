namespace Web.Repository;

public interface IUserRepository<T> {
  Task<T> GetUser (string email, string password);
  void Add(T entity);
  Task<T> GetUserByEmail(string email);
  Task<T> GetUserByEmailActive(string email);
  Task<T> GetUserByToken(string token);
  Task<T> GetUserActiveByToken(string token);
  void Update(T entity);
}