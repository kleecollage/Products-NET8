using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

namespace Web.Repository;

public class UserRepository(ApplicationDbContext context): IUserRepository<Usuario>
{
  private readonly ApplicationDbContext _context = context;

    public void Add(Usuario entity)
    {
        _context.Set<Usuario>().Add(entity);
        _context.SaveChanges();
    }

    public async Task<Usuario> GetUser(string email, string password)
    {
        return await _context.Usuarios
          .Where(u => u.Correo == email)
          .Where(p => p.Password == password)
          .Where(e => e.Estado == 1)
          .FirstOrDefaultAsync();
    }

    public async Task<Usuario> GetUserActiveByToken(string token)
    {
        return await _context.Usuarios
          .Where(u => u.Token == token)
          .Where(e => e.Estado == 1)
          .FirstOrDefaultAsync();
    }

    public async Task<Usuario> GetUserByEmail(string email)
    {
      return await _context.Usuarios.Where(u => u.Correo == email).FirstOrDefaultAsync();
    }

    public async Task<Usuario> GetUserByEmailActive(string email)
    {
        return await _context.Usuarios
          .Where(u => u.Correo == email)
          .Where(e => e.Estado == 1)
          .FirstOrDefaultAsync();
    }

    public async Task<Usuario> GetUserByToken(string token)
    {
        return await _context.Usuarios
          .Where(u => u.Token == token)
          .Where(e => e.Estado == 0)
          .FirstOrDefaultAsync();
    }

    public void Update(Usuario entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }
}
