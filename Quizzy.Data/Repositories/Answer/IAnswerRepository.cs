using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public interface IAnswerRepository
{
    Task<Answer> CreateAsync(Answer answer);
    Task CreateRangeAsync(IEnumerable<Answer> answer);
    Answer Update(Answer answer);
    Task<bool> DeleteAsync(Expression<Func<Answer, bool>> expression);
    Task RemoveRangeAsync(IEnumerable<Answer> answers);
    Task<Answer?> GetByIdAsync(int id);
    Task<IEnumerable<Answer>> GetAllByQuestionIdAsync(int questionId);
    IQueryable<Answer> Where(Expression<Func<Answer, bool>>? expression = null);
}
