using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Question;
using Quizzy.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Quizzy.Api.Controllers;

[ApiController]
[Route("api/questions")]
public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> GetAllQuestions(
        [FromQuery] PaginationParameters? parameters)
    {
        return Ok(await questionService.GetAllAsync(parameters: parameters));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{questionId}")]
    public async Task<ActionResult<QuestionViewDto>> GetQuestion(int questionId)
    {
        return Ok(await questionService.GetAsync(questionId));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost]
    public async Task<ActionResult<QuestionViewDto>> CreateQuestion(QuestionCreationDto questionCreationDto)
    {
        return Ok(await questionService.CreateAsync(questionCreationDto));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPatch("{questionId}")]
    public async Task<ActionResult<Question>> UpdateQuestion([Required] int questionId, Question question)
    {
        return Ok(await questionService.UpdateAsync(questionId, question));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{questionId}")]
    public async Task<ActionResult<bool>> DeleteQuestion(uint questionId)
    {
        return Ok(await questionService.DeleteAsync(question => question.Id == questionId));
    }
}