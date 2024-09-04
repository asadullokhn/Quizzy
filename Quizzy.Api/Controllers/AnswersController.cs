using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Answer;
using Quizzy.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Quizzy.Api.Controllers;

[ApiController]
[Route("api/answers")]
public class AnswersController(IAnswerService answerService) : ControllerBase
{
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{answerId}")]
    public async Task<ActionResult<Answer>> GetAnswer(int answerId)
    {
        return Ok(await answerService.GetAsync(answerId));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("/api/questions/{questionId}/answers")]
    public async Task<ActionResult<IEnumerable<Answer>>> GetQuestionAllAnswers(int questionId)
    {
        return Ok(await answerService.GetQuestionAllAnswers(questionId));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("/api/questions/{questionId}/answers")]
    public async Task<ActionResult<Answer>> AddQuestionAnswers([Required] int questionId, AnswerCreationDto answerCreation)
    {
        return Ok(await answerService.AddAsync(questionId, answerCreation));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPatch("{answerId}")]
    public async Task<ActionResult<Answer>> UpdateAnswer([Required] int answerId, Answer answer)
    {
        return Ok(await answerService.UpdateAsync(answerId, answer));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{answerId}")]
    public async Task<ActionResult<bool>> DeleteAnswer(uint answerId)
    {
        return Ok(await answerService.DeleteAsync(answer => answer.Id == answerId));
    }
}