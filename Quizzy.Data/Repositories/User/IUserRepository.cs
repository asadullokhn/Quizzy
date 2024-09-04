using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    User Update(User user);
    Task<bool> DeleteAsync(Expression<Func<User, bool>> expression);
    Task RemoveRangeAsync(IEnumerable<User> users);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByEmailAndPasswordAsync(string email, string password);
    Task<User?> GetByIdAsync(int id);
    Task<bool> AnyAsync(string email);
    IQueryable<User> Where(Expression<Func<User, bool>>? expression = null);
}
