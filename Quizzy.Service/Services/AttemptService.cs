using Quizzy.Data.Repositories;
using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Attempt;
using Quizzy.Service.Exceptions;
using Quizzy.Service.Extensions;
using Quizzy.Service.Interfaces;
using System.Linq.Expressions;

namespace Quizzy.Service.Services;

public class AttemptService(IUnitOfWork unitOfWork) : IAttemptService
{
    public async Task<AttemptViewDto> StartQuizAttemptAsync(int userId, int quizId)
    {
        var quiz = await unitOfWork.Quizzes.GetByIdAsync(quizId)
            ?? throw new BadRequestException("Quiz does not exist");

        var attempt = await unitOfWork.Attempts.GetValidAttemptByUserIdAsync(userId);
        if (attempt == null)
        {
            attempt = new Attempt()
            {
                QuizId = quizId,
                UserId = userId,
                ExpireAt = DateTime.UtcNow.AddMinutes(quiz.DurationInMinutes),
                StartedAt = DateTime.UtcNow
            };
            attempt = await unitOfWork.Attempts.CreateAsync(attempt);
            await unitOfWork.SaveChangesAsync();
        }

        if (attempt.QuizId != quizId) throw new BadRequestException("You have another active Quiz Session");

        var choosenAsnwers = await unitOfWork.AttemptAnswers.GetAllAnswersByAttemptIdAsync(attempt.Id);
        var questions = await unitOfWork.QuizQuestions.GetAllQuestionsByQuizIdAsync(quizId);
        var result = new List<AttemptQuestionViewDto>();

        foreach (var question in questions)
        {
            var answers = await unitOfWork.Answers.GetAllByQuestionIdAsync(question.Id);
            result.Add(new AttemptQuestionViewDto()
            {
                Id = question.Id,
                Title = question.Title,
                Description = question.Description,
                WasSubmitted = choosenAsnwers.Any(ch => ch.QuestionId == question.Id),
                Answers = answers.Select(s => new AttemptAnswerViewDto()
                {
                    Id = s.Id,
                    Content = s.Content,
                    WasChoosen = choosenAsnwers.Any(ch => ch.Id == s.Id)
                }).ToList()
            });
        }

        return new AttemptViewDto()
        {
            Id = attempt.Id,
            Questions = result,
            DurationInMinutes = quiz.DurationInMinutes,
            StartedAt = attempt.StartedAt,
            ExpireAt = attempt.ExpireAt,
        };
    }

    public async Task<bool> EndQuizAttemptAsync(int userId)
    {
        var attempt = await unitOfWork.Attempts.GetValidAttemptByUserIdAsync(userId)
            ?? throw new NotFoundException("Attempt");

        attempt.FinishedAt = DateTime.UtcNow;
        unitOfWork.Attempts.Update(attempt);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<Attempt> GetAsync(int id)
        => await unitOfWork.Attempts.GetByIdAsync(id) ?? throw new NotFoundException("Attempt");

    public Task<IEnumerable<Attempt>> GetAllAsync(Expression<Func<Attempt, bool>>? expression = null,
        PaginationParameters? parameters = null)
        => Task.FromResult<IEnumerable<Attempt>>(unitOfWork.Attempts.Where(expression).ToPagedAsQueryable(parameters));

    public async Task<bool> SubmitAttemptAsync(int userId, int[] answerIds)
    {
        var attempt = await unitOfWork.Attempts.GetValidAttemptByUserIdAsync(userId)
            ?? throw new BadRequestException("Active Quiz session does not exist");

        var answers = new List<Answer>();
        foreach (var answerId in answerIds)
        {
            var answer = await unitOfWork.Answers.GetByIdAsync(answerId)
                ?? throw new BadRequestException("Answer does not exist");
            answers.Add(answer);
        }

        var questionId = answers.First().QuestionId;
        if (!answers.All(a => a.QuestionId == questionId))
            throw new BadRequestException("Answers must belong to the same question");

        var anyQuizQuestion = await unitOfWork.QuizQuestions.AnyAsync(attempt.QuizId, questionId);
        if (!anyQuizQuestion) throw new BadRequestException("This Answer in your current quiz does not exist");

        var attemptedAnswers = await unitOfWork.AttemptAnswers.GetAllAnswersByAttemptIdAsync(attempt.Id);
        if (attemptedAnswers.Any(att => att.QuestionId == questionId))
            throw new BadRequestException("This Question is already submitted");

        foreach (var answer in answers)
        {
            await unitOfWork.AttemptAnswers.CreateAsync(new AttemptAnswers()
            {
                AttemptId = attempt.Id,
                AnswerId = answer.Id
            });

            await unitOfWork.SaveChangesAsync();
        }

        return true;
    }

    public async Task<IEnumerable<AttemptResultDto>> GetAttemptResultsAsync(int userId, int quizId)
    {
        var attempts = await unitOfWork.Attempts.GetAllAttemptsByUserIdAndQuizId(userId, quizId)
            ?? throw new NotFoundException("Attempt");

        var results = new List<AttemptResultDto>();
        foreach (var attempt in attempts)
        {
            var quizQuestions = await unitOfWork.QuizQuestions.GetAllQuestionsByQuizIdAsync(quizId);
            var userAnswers = await unitOfWork.AttemptAnswers.GetAllAnswersByAttemptIdAsync(attempt.Id);

            double points = quizQuestions.Sum(question
                => (double)userAnswers.Where(p => p.QuestionId == question.Id).Count(c => c.IsRight) / question.RightAnswersCount);

            results.Add(new AttemptResultDto()
            {
                Id = attempt.Id,
                ExpireAt = attempt.ExpireAt,
                FinishedAt = attempt.FinishedAt,
                StartedAt = attempt.StartedAt,
                RightAnswers = points,
                LeftAnswers = quizQuestions.Count() - points,
                RightAnswersPercentage = userAnswers.Any()
                   ? 100.0 / quizQuestions.Count() * points
                   : 0.0,
                LeftAnswersPercentage = userAnswers.Any()
                   ? 100 - (100.0 / quizQuestions.Count() * points)
                   : 100.0,
            });

        }

        return results;
    }
}
