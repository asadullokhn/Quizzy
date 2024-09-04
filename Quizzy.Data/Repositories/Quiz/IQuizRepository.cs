using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public interface IQuizRepository
{
    Task<Quiz> CreateAsync(Quiz quiz);
    Quiz Update(Quiz quiz);
    Task<bool> DeleteAsync(Expression<Func<Quiz, bool>> expression);
    Task RemoveRangeAsync(IEnumerable<Quiz> quizs);
    Task<Quiz?> GetByIdAsync(int id);
    Task<bool> AnyAsync(int id);
    IQueryable<Quiz> Where(Expression<Func<Quiz, bool>>? expression = null);
}