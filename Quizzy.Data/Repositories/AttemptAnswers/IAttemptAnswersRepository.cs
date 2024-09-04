using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public interface IAttemptAnswersRepository
{
    Task<AttemptAnswers> CreateAsync(AttemptAnswers attemptAnswers);
    Task<IEnumerable<Answer>> GetAllAnswersByAttemptIdAsync(int attemptId);
    IQueryable<AttemptAnswers> Where(Expression<Func<AttemptAnswers, bool>>? expression = null);
}
