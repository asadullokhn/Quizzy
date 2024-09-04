using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Attempt;
using System.Linq.Expressions;

namespace Quizzy.Service.Interfaces;

public interface IAttemptService
{
    Task<AttemptViewDto> StartQuizAttemptAsync(int userId, int quizId);

    Task<bool> EndQuizAttemptAsync(int userId);

    Task<bool> SubmitAttemptAsync(int userId, int[] answerIds);

    Task<Attempt> GetAsync(int id);

    Task<IEnumerable<Attempt>> GetAllAsync(Expression<Func<Attempt, bool>>? expression = null,
        PaginationParameters? parameters = null);

    Task<IEnumerable<AttemptResultDto>> GetAttemptResultsAsync(int userId, int quizId);
}