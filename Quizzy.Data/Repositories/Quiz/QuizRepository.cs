using Microsoft.EntityFrameworkCore;
using Quizzy.Data.Contexts;
using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public class QuizRepository(QuizzyDbContext dbContext) : IQuizRepository
{
    public Quiz Update(Quiz entity)
    => dbContext.Quizzes.Update(entity).Entity;

    public async Task<Quiz> CreateAsync(Quiz entity)
        => (await dbContext.Quizzes.AddAsync(entity)).Entity;

    public Task<Quiz?> GetByIdAsync(int id) => dbContext.Quizzes.FirstOrDefaultAsync(p => p.Id == id);
    public IQueryable<Quiz> Where(Expression<Func<Quiz, bool>>? expression = null)
        => expression is null ? dbContext.Quizzes : dbContext.Quizzes.Where(expression);

    public async Task<bool> DeleteAsync(Expression<Func<Quiz, bool>> expression)
    {
        var entity = await dbContext.Quizzes.FirstOrDefaultAsync(expression);

        if (entity is null)
            return false;

        dbContext.Quizzes.Remove(entity);
        return true;
    }

    public Task RemoveRangeAsync(IEnumerable<Quiz> users)
    {
        dbContext.Quizzes.RemoveRange(users);
        return Task.CompletedTask;
    }

    public Task<bool> AnyAsync(int id)
        => dbContext.Quizzes.AnyAsync(q => q.Id == id);
}
