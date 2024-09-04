using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public interface IAttemptRepository
{
    Task<Attempt> CreateAsync(Attempt attempt);
    Attempt Update(Attempt attempt);
    Task<bool> DeleteAsync(Expression<Func<Attempt, bool>> expression);
    Task RemoveRangeAsync(IEnumerable<Attempt> attempts);
    Task<Attempt?> GetByIdAsync(int id);
    Task<Attempt?> GetValidAttemptByUserIdAsync(int userId);
    IQueryable<Attempt> Where(Expression<Func<Attempt, bool>>? expression = null);
    Task<IEnumerable<Attempt>> GetAllAttemptsByUserIdAndQuizId(int userId, int quizId);
    Task<IEnumerable<Attempt>> GetAllAttemptsByUserId(int userId);
}
