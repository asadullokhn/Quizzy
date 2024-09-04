namespace Quizzy.Data.Repositories;

public interface IUnitOfWork
{
    IAnswerRepository Answers { get; }
    IAttemptRepository Attempts { get; }
    IAttemptAnswersRepository AttemptAnswers { get; }
    IQuestionRepository Questions { get; }
    IQuizRepository Quizzes { get; }
    IQuizQuestionsRepository QuizQuestions { get; }
    IUserRepository Users { get; }

    Task SaveChangesAsync();
}
