using Microsoft.EntityFrameworkCore;
using Quizzy.Data.Contexts;
using Quizzy.Domain.Entities;
using System.Linq.Expressions;

namespace Quizzy.Data.Repositories;

public class QuizQuestionsRepository(QuizzyDbContext dbContext) : IQuizQuestionsRepository
{
    public QuizQuestions Update(QuizQuestions entity)
        => dbContext.QuizQuestions.Update(entity).Entity;

    public async Task<QuizQuestions> CreateAsync(QuizQuestions entity, bool skipDublicates = true)
    {
        if (skipDublicates)
        {
            var quizQuestion = await dbContext.QuizQuestions.FirstOrDefaultAsync(p => p.QuizId == entity.QuizId && p.QuestionId == entity.QuestionId);
            if (quizQuestion != null) return quizQuestion;
        }

        return (await dbContext.QuizQuestions.AddAsync(entity)).Entity;
    }

    public Task<QuizQuestions?> GetByIdAsync(int id) => dbContext.QuizQuestions.FirstOrDefaultAsync(p => p.Id == id);
    public IQueryable<QuizQuestions> Where(Expression<Func<QuizQuestions, bool>>? expression = null)
        => expression is null ? dbContext.QuizQuestions : dbContext.QuizQuestions.Where(expression);

    public Task RemoveRangeAsync(int quizId, int[] questionIds)
    {
        dbContext.QuizQuestions.RemoveRange(
            dbContext.QuizQuestions.Where(q => q.QuizId == quizId && questionIds.Contains(q.QuestionId)));

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<Question>> GetAllQuestionsByQuizIdAsync(int quizId)
        => await dbContext.QuizQuestions
            .Where(q => q.QuizId == quizId)
            .Include(i => i.Question)
            .Select(s => s.Question!)
            .AsNoTracking()
        .ToListAsync();

    public Task<int[]> GetAllQuestionIdsByQuizIdAsync(int quizId)
    {
        return Task.FromResult<int[]>
         ([.. dbContext.QuizQuestions
            .Where(q => q.QuizId == quizId)
            .AsNoTracking()
            .Select(s => s.QuestionId)]);
    }

    public Task<bool> AnyAsync(int quizId, int questionId)
        => dbContext.QuizQuestions.AnyAsync(q => q.QuestionId == questionId && q.QuizId == quizId);
}
