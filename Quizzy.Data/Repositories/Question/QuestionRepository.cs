using Microsoft.EntityFrameworkCore;
using Quizzy.Data.Contexts;
using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public class QuestionRepository(QuizzyDbContext dbContext) : IQuestionRepository
{
    public Question Update(Question entity)
    => dbContext.Questions.Update(entity).Entity;

    public async Task<Question> CreateAsync(Question entity)
        => (await dbContext.Questions.AddAsync(entity)).Entity;

    public Task<Question?> GetByIdAsync(int id) => dbContext.Questions.FirstOrDefaultAsync(p => p.Id == id);
    public IQueryable<Question> Where(Expression<Func<Question, bool>>? expression = null)
        => expression is null ? dbContext.Questions : dbContext.Questions.Where(expression);

    public async Task<bool> DeleteAsync(Expression<Func<Question, bool>> expression)
    {
        var entity = await dbContext.Questions.FirstOrDefaultAsync(expression);

        if (entity is null)
            return false;

        dbContext.Questions.Remove(entity);
        return true;
    }

    public Task RemoveRangeAsync(IEnumerable<Question> users)
    {
        dbContext.Questions.RemoveRange(users);
        return Task.CompletedTask;
    }

    public Task<bool> AnyAsync(int id)
         => dbContext.Questions.AnyAsync(q => q.Id.Equals(id));

    public async Task<Question> UpdateCountAsync(int id, int count)
    {
        var question = await dbContext.Questions.FirstAsync(p => p.Id == id);
        question.RightAnswersCount += count;

        return question;
    }
}
