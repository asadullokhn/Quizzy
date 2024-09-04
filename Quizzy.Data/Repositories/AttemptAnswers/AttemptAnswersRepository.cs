using Microsoft.EntityFrameworkCore;
using Quizzy.Data.Contexts;
using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public class AttemptAnswersRepository(QuizzyDbContext dbContext) : IAttemptAnswersRepository
{
    public async Task<AttemptAnswers> CreateAsync(AttemptAnswers entity)
        => (await dbContext.AttemptAnswers.AddAsync(entity)).Entity;

    public IQueryable<AttemptAnswers> Where(Expression<Func<AttemptAnswers, bool>>? expression = null)
        => expression is null ? dbContext.AttemptAnswers : dbContext.AttemptAnswers.Where(expression);

    public Task<IEnumerable<Answer>> GetAllAnswersByAttemptIdAsync(int attemptId)
        => Task.FromResult<IEnumerable<Answer>>(
            dbContext.AttemptAnswers
            .Where(p => p.AttemptId == attemptId)
            .Include(i => i.Answer)
            .Select(s => s.Answer!));
}
