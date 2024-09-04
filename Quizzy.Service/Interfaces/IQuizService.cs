using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Quiz;
using System.Linq.Expressions;

namespace Quizzy.Service.Interfaces;

public interface IQuizService
{
    Task<QuizViewDto> CreateAsync(int creatorId, QuizCreationDto quizForCreationDto);

    Task<bool> AddQuizQuestionsAsync(int id, int[] questionIds);

    Task<bool> RemoveQuizQuestionsAsync(int id, int[] questionIds);

    Task<IEnumerable<Question>> GetQuizQuestionsAsync(int quizId);

    Task<Quiz> UpdateAsync(int id, QuizCreationDto quiz);

    Task<bool> DeleteAsync(Expression<Func<Quiz, bool>> expression);

    Task<Quiz> GetAsync(int id);

    Task<IEnumerable<Quiz>> GetAllAsync(Expression<Func<Quiz, bool>>? expression = null,
        PaginationParameters? parameters = null);
}