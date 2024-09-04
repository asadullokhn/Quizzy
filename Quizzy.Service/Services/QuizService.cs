using AutoMapper;
using Quizzy.Data.Repositories;
using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Quiz;
using Quizzy.Service.Exceptions;
using Quizzy.Service.Extensions;
using Quizzy.Service.Interfaces;
using System.Linq.Expressions;

namespace Quizzy.Service.Services;

public class QuizService(IUnitOfWork unitOfWork, IMapper mapper) : IQuizService
{
    public async Task<QuizViewDto> CreateAsync(int creatorId, QuizCreationDto quizForCreationDto)
    {
        var quiz = mapper.Map<Quiz>(quizForCreationDto);
        quiz.CreatedByUserId = creatorId;

        quiz = await unitOfWork.Quizzes.CreateAsync(quiz);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<QuizViewDto>(quiz);
    }

    public async Task<Quiz> UpdateAsync(int id, QuizCreationDto quiz)
    {
        var existQuiz = await unitOfWork.Quizzes.GetByIdAsync(id)
            ?? throw new NotFoundException("Quiz");

        existQuiz.Name = quiz.Name;
        existQuiz.Description = quiz.Description;

        unitOfWork.Quizzes.Update(existQuiz);
        await unitOfWork.SaveChangesAsync();

        return existQuiz;
    }

    public async Task<bool> DeleteAsync(Expression<Func<Quiz, bool>> expression)
    {
        var quizs = unitOfWork.Quizzes.Where(expression);

        if (!quizs.Any())
            throw new NotFoundException("Quiz");

        await unitOfWork.Quizzes.RemoveRangeAsync(quizs);
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<Quiz> GetAsync(int id)
        => await unitOfWork.Quizzes.GetByIdAsync(id) ?? throw new NotFoundException("Quiz");

    public Task<IEnumerable<Quiz>> GetAllAsync(Expression<Func<Quiz, bool>>? expression = null,
        PaginationParameters? parameters = null)
        => Task.FromResult<IEnumerable<Quiz>>(unitOfWork.Quizzes.Where(expression)
                .ToPagedAsQueryable(parameters));

    public async Task<bool> AddQuizQuestionsAsync(int id, int[] questionIds)
    {
        var quiz = await unitOfWork.Quizzes.AnyAsync(id);
        if (!quiz) throw new BadRequestException("Quiz does not exist");

        foreach (var qId in questionIds)
            if (!await unitOfWork.Questions.AnyAsync(qId))
                throw new BadRequestException($"Question with {qId} id does not exist");

        foreach (var qId in questionIds)
            await unitOfWork.QuizQuestions.CreateAsync(new QuizQuestions() { QuizId = id, QuestionId = qId });

        await unitOfWork.SaveChangesAsync();

        return true;
    }
    public async Task<bool> RemoveQuizQuestionsAsync(int id, int[] questionIds)
    {
        var quiz = await unitOfWork.Quizzes.AnyAsync(id);
        if (!quiz) throw new BadRequestException("Quiz does not exist");

        foreach (var qId in questionIds)
            if (!await unitOfWork.Questions.AnyAsync(qId))
                throw new BadRequestException($"Question with {qId} id does not exist");

        await unitOfWork.QuizQuestions.RemoveRangeAsync(id, questionIds);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
    public async Task<IEnumerable<Question>> GetQuizQuestionsAsync(int quizId)
    {
        var quiz = await unitOfWork.Quizzes.AnyAsync(quizId);
        if (!quiz) throw new BadRequestException("Quiz does not exist");

        return await unitOfWork.QuizQuestions.GetAllQuestionsByQuizIdAsync(quizId);
    }
}
