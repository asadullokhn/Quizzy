using Microsoft.EntityFrameworkCore;
using Quizzy.Data.Contexts;
using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public class AttemptRepository(QuizzyDbContext dbContext) : IAttemptRepository
{
    public Attempt Update(Attempt entity)
    => dbContext.Attempts.Update(entity).Entity;

    public async Task<Attempt> CreateAsync(Attempt entity)
        => (await dbContext.Attempts.AddAsync(entity)).Entity;

    public Task<Attempt?> GetByIdAsync(int id) => dbContext.Attempts.FirstOrDefaultAsync(p => p.Id == id);
    public IQueryable<Attempt> Where(Expression<Func<Attempt, bool>>? expression = null)
        => expression is null ? dbContext.Attempts : dbContext.Attempts.Where(expression);

    public async Task<bool> DeleteAsync(Expression<Func<Attempt, bool>> expression)
    {
        var entity = await dbContext.Attempts.FirstOrDefaultAsync(expression);

        if (entity is null)
            return false;

        dbContext.Attempts.Remove(entity);
        return true;
    }

    public Task RemoveRangeAsync(IEnumerable<Attempt> users)
    {
        dbContext.Attempts.RemoveRange(users);
        return Task.CompletedTask;
    }

    public Task<Attempt?> GetValidAttemptByUserIdAsync(int userId)
    {
        var now = DateTime.UtcNow;
        return dbContext.Attempts.FirstOrDefaultAsync(a => a.UserId == userId && a.ExpireAt > now && a.FinishedAt == null);
    }

    public async Task<IEnumerable<Attempt>> GetAllAttemptsByUserIdAndQuizId(int userId, int quizId)
    {
        return await dbContext.Attempts.Where(a => a.UserId == userId && a.QuizId == quizId).ToListAsync();
    }

    public async Task<IEnumerable<Attempt>> GetAllAttemptsByUserId(int userId)
    {
        return await dbContext.Attempts.Where(a => a.UserId == userId).ToListAsync();
    }
}
