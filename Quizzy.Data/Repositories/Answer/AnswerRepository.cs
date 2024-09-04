using Microsoft.EntityFrameworkCore;
using Quizzy.Data.Contexts;
using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public class AnswerRepository(QuizzyDbContext dbContext) : IAnswerRepository
{
    public Answer Update(Answer entity)
    => dbContext.Answers.Update(entity).Entity;

    public async Task<Answer> CreateAsync(Answer entity)
        => (await dbContext.Answers.AddAsync(entity)).Entity;

    public Task CreateRangeAsync(IEnumerable<Answer> entity)
        => dbContext.Answers.AddRangeAsync(entity);

    public Task<Answer?> GetByIdAsync(int id) => dbContext.Answers.FirstOrDefaultAsync(p => p.Id == id);
    public IQueryable<Answer> Where(Expression<Func<Answer, bool>>? expression = null)
        => expression is null ? dbContext.Answers : dbContext.Answers.Where(expression);

    public async Task<bool> DeleteAsync(Expression<Func<Answer, bool>> expression)
    {
        var entity = await dbContext.Answers.FirstOrDefaultAsync(expression);

        if (entity is null)
            return false;

        dbContext.Answers.Remove(entity);
        return true;
    }

    public Task RemoveRangeAsync(IEnumerable<Answer> users)
    {
        dbContext.Answers.RemoveRange(users);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Answer>> GetAllByQuestionIdAsync(int questionId)
    {
        return await dbContext.Answers.Where(p => p.QuestionId == questionId).AsNoTracking().ToListAsync();
    }
}
