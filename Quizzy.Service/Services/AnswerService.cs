using AutoMapper;
using Quizzy.Data.Repositories;
using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Answer;
using Quizzy.Service.Exceptions;
using Quizzy.Service.Extensions;
using Quizzy.Service.Interfaces;
using System.Linq.Expressions;

namespace Quizzy.Service.Services;

public class AnswerService(IUnitOfWork unitOfWork, IMapper mapper) : IAnswerService
{
    public async Task<Answer> AddAsync(int questionId, AnswerCreationDto answerCreationDto)
    {
        var question = await unitOfWork.Questions.AnyAsync(questionId);
        if (!question)
            throw new BadRequestException("Question does not exist");

        var answer = mapper.Map<Answer>(answerCreationDto);
        answer.QuestionId = questionId;

        answer = await unitOfWork.Answers.CreateAsync(answer);
        await unitOfWork.SaveChangesAsync();

        if (answer.IsRight)
            await unitOfWork.Questions.UpdateCountAsync(questionId, 1);

        return answer;
    }

    public async Task<Answer> UpdateAsync(int id, Answer answer)
    {
        var existAnswer = await unitOfWork.Answers.GetByIdAsync(id)
            ?? throw new NotFoundException("Answer");

        // Updates goes here
        // answer -> existAnswer
        unitOfWork.Answers.Update(existAnswer);
        await unitOfWork.SaveChangesAsync();

        return existAnswer;
    }

    public async Task<bool> DeleteAsync(Expression<Func<Answer, bool>> expression)
    {
        var answers = unitOfWork.Answers.Where(expression);
        if (!answers.Any())
            throw new NotFoundException("Answer");

        foreach (var answer in answers)
        {
            if (answer.IsRight)
            {
                await unitOfWork.Questions.UpdateCountAsync(answer.QuestionId, -1);
                await unitOfWork.SaveChangesAsync();
            }

        }

        await unitOfWork.Answers.RemoveRangeAsync(answers);
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<Answer> GetAsync(int id)
        => await unitOfWork.Answers.GetByIdAsync(id) ?? throw new NotFoundException("Answer");

    public Task<IEnumerable<Answer>> GetAllAsync(Expression<Func<Answer, bool>>? expression = null,
        PaginationParameters? parameters = null)
        => Task.FromResult<IEnumerable<Answer>>(unitOfWork.Answers.Where(expression).ToPagedAsQueryable(parameters));

    public Task<IEnumerable<Answer>> GetQuestionAllAnswers(int questionId)
    {
        return unitOfWork.Answers.GetAllByQuestionIdAsync(questionId);
    }
}
