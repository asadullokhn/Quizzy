using Quizzy.Data.Repositories;
using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Answer;
using Quizzy.Service.DTOs.Question;
using Quizzy.Service.Exceptions;
using Quizzy.Service.Extensions;
using Quizzy.Service.Interfaces;
using System.Linq.Expressions;

namespace Quizzy.Service.Services;

public class QuestionService(IUnitOfWork unitOfWork) : IQuestionService
{
    public async Task<QuestionViewDto> CreateAsync(QuestionCreationDto questionCreationDto)
    {
        var question = new Question()
        {
            Title = questionCreationDto.Title,
            Description = questionCreationDto.Description,
            RightAnswersCount = questionCreationDto.Answers.Count(c => c.IsRight),
        };
        if (question.RightAnswersCount < 1)
            throw new BadRequestException("Your question must have at least one right answer");

        question = await unitOfWork.Questions.CreateAsync(question);
        await unitOfWork.SaveChangesAsync();

        var answers = questionCreationDto.Answers.Select(s => new Answer()
        {
            Content = s.Content,
            IsRight = s.IsRight,
            QuestionId = question.Id
        });

        await unitOfWork.Answers.CreateRangeAsync(answers);
        await unitOfWork.SaveChangesAsync();

        return new QuestionViewDto()
        {
            Id = question.Id,
            Title = question.Title,
            Description = question.Description,
            Answers = answers.Select(s => new AnswerViewDto() { Id = s.Id, Content = s.Content })
        };
    }

    public async Task<Question> UpdateAsync(int id, Question question)
    {
        var existQuestion = await unitOfWork.Questions.GetByIdAsync(id)
            ?? throw new NotFoundException("Question");

        existQuestion.Title = question.Title;
        existQuestion.Description = question.Description;

        unitOfWork.Questions.Update(existQuestion);
        await unitOfWork.SaveChangesAsync();

        return existQuestion;
    }

    public async Task<bool> DeleteAsync(Expression<Func<Question, bool>> expression)
    {
        var questions = unitOfWork.Questions.Where(expression);

        if (!questions.Any())
            throw new NotFoundException("Question");


        await unitOfWork.Questions.RemoveRangeAsync(questions);
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<QuestionViewDto> GetAsync(int id)
    {
        var question = await unitOfWork.Questions.GetByIdAsync(id) ?? throw new NotFoundException("Question");

        var answers = await unitOfWork.Answers.GetAllByQuestionIdAsync(id);

        return new QuestionViewDto()
        {
            Id = question.Id,
            Title = question.Title,
            Description = question.Description,
            Answers = answers.Select(s => new AnswerViewDto() { Id = s.Id, Content = s.Content }).ToArray()
        };
    }

    public Task<IEnumerable<Question>> GetAllAsync(Expression<Func<Question, bool>>? expression = null,
        PaginationParameters? parameters = null)
        => Task.FromResult<IEnumerable<Question>>(unitOfWork.Questions.Where(expression).ToPagedAsQueryable(parameters));
}
