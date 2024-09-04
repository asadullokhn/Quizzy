using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Answer;
using System.Linq.Expressions;

namespace Quizzy.Service.Interfaces;

public interface IAnswerService
{
    Task<Answer> AddAsync(int questionId, AnswerCreationDto answerCreationDto);

    Task<Answer> UpdateAsync(int id, Answer answer);

    Task<bool> DeleteAsync(Expression<Func<Answer, bool>> expression);

    Task<Answer> GetAsync(int id);

    Task<IEnumerable<Answer>> GetQuestionAllAnswers(int questionId);

    Task<IEnumerable<Answer>> GetAllAsync(Expression<Func<Answer, bool>>? expression = null,
        PaginationParameters? parameters = null);
}