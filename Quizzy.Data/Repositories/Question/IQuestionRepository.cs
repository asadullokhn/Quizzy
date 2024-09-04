using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public interface IQuestionRepository
{
    Task<bool> AnyAsync(int id);
    Task<Question> CreateAsync(Question question);
    Question Update(Question question);
    Task<Question> UpdateCountAsync(int id, int count);
    Task<bool> DeleteAsync(Expression<Func<Question, bool>> expression);
    Task RemoveRangeAsync(IEnumerable<Question> questions);
    Task<Question?> GetByIdAsync(int id);
    IQueryable<Question> Where(Expression<Func<Question, bool>>? expression = null);
}
