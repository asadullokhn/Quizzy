using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Attempt;
using Quizzy.Service.Helpers;
using Quizzy.Service.Interfaces;

namespace Quizzy.Api.Controllers;

[ApiController]
[Route("api/quizzes/attempts")]
public class AttemptsController(IAttemptService attemptService) : ControllerBase
{
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attempt>>> GetAllAttempts(
        [FromQuery] PaginationParameters? parameters)
    {
        return Ok(await attemptService.GetAllAsync(parameters: parameters));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{attemptId}")]
    public async Task<ActionResult<Attempt>> GetAttempt(int attemptId)
    {
        return Ok(await attemptService.GetAsync(attemptId));
    }

    [Authorize(Policy = "UserPolicy")]
    [HttpPost("/api/quizzes/{quizId}/start")]
    public async Task<ActionResult<AttemptViewDto>> StartQuizAttempt(int quizId)
    {
        return Ok(await attemptService.StartQuizAttemptAsync(HttpContextHelper.UserId, quizId));
    }

    [Authorize(Policy = "UserPolicy")]
    [HttpPost("submit")]
    public async Task<ActionResult<bool>> SubmitAttempt(int[] answerIds)
    {
        if (answerIds.Length < 1) { return BadRequest("Provide at least one Answer Id"); }

        return Ok(await attemptService.SubmitAttemptAsync(HttpContextHelper.UserId, answerIds));
    }

    [Authorize(Policy = "UserPolicy")]
    [HttpPost("finish")]
    public async Task<ActionResult<bool>> EndQuizAttempt()
    {
        return Ok(await attemptService.EndQuizAttemptAsync(HttpContextHelper.UserId));
    }


    [Authorize(Policy = "UserPolicy")]
    [HttpPost("/api/quizzes/{quizId}/results")]
    public async Task<ActionResult<IEnumerable<AttemptResultDto>>> GetAttemptResults(int quizId)
    {
        return Ok(await attemptService.GetAttemptResultsAsync(HttpContextHelper.UserId, quizId));
    }
}