using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public interface IQuizQuestionsRepository
{
    Task<QuizQuestions> CreateAsync(QuizQuestions quizQuestions, bool skipDublicates = true);
    QuizQuestions Update(QuizQuestions quizQuestions);
    Task RemoveRangeAsync(int quizId, int[] questionIds);
    Task<QuizQuestions?> GetByIdAsync(int id);
    Task<IEnumerable<Question>> GetAllQuestionsByQuizIdAsync(int quizId);
    Task<int[]> GetAllQuestionIdsByQuizIdAsync(int quizId);
    Task<bool> AnyAsync(int quizId, int questionId);
    IQueryable<QuizQuestions> Where(Expression<Func<QuizQuestions, bool>>? expression = null);
}
