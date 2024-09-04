using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Question;
using System.Linq.Expressions;

namespace Quizzy.Service.Interfaces;

public interface IQuestionService
{
    Task<QuestionViewDto> CreateAsync(QuestionCreationDto questionCreationDto);

    Task<Question> UpdateAsync(int id, Question question);

    Task<bool> DeleteAsync(Expression<Func<Question, bool>> expression);

    Task<QuestionViewDto> GetAsync(int id);

    Task<IEnumerable<Question>> GetAllAsync(Expression<Func<Question, bool>>? expression = null,
        PaginationParameters? parameters = null);
}