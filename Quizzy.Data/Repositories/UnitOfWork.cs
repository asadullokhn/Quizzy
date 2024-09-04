using Quizzy.Data.Contexts;

namespace Quizzy.Data.Repositories;

public class UnitOfWork(QuizzyDbContext dbContext) : IUnitOfWork
{
    public IUserRepository Users { get; } = new UserRepository(dbContext);

    public IAttemptRepository Attempts { get; } = new AttemptRepository(dbContext);

    public IAnswerRepository Answers => new AnswerRepository(dbContext);

    public IAttemptAnswersRepository AttemptAnswers => new AttemptAnswersRepository(dbContext);

    public IQuestionRepository Questions => new QuestionRepository(dbContext);

    public IQuizRepository Quizzes => new QuizRepository(dbContext);

    public IQuizQuestionsRepository QuizQuestions => new QuizQuestionsRepository(dbContext);

    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}