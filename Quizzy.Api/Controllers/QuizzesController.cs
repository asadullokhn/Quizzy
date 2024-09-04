using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.Quiz;
using Quizzy.Service.Helpers;
using Quizzy.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Quizzy.Api.Controllers;

[ApiController]
[Route("api/quizzes")]
public class QuizzesController(IQuizService quizService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Quiz>>> GetAllQuizs(
        [FromQuery] PaginationParameters? parameters)
    {
        return Ok(await quizService.GetAllAsync(parameters: parameters));
    }

    [Authorize(Policy = "UserPolicy")]
    [HttpGet("{quizId}")]
    public async Task<ActionResult<Quiz>> GetQuiz(int quizId)
    {
        return Ok(await quizService.GetAsync(quizId));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{quizId}/questions")]
    public async Task<ActionResult<IEnumerable<Question>>> GetQuizQuestions(int quizId)
    {
        return Ok(await quizService.GetQuizQuestionsAsync(quizId));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost]
    public async Task<ActionResult<QuizViewDto>> CreateQuiz(QuizCreationDto quizForCreationDto)
    {
        return Ok(await quizService.CreateAsync(HttpContextHelper.UserId, quizForCreationDto));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{quizId}/questions")]
    public async Task<ActionResult<bool>> AddQuizQuestions(int quizId, int[] questionIds)
    {
        return Ok(await quizService.AddQuizQuestionsAsync(quizId, questionIds));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPatch("{quizId}")]
    public async Task<ActionResult<Quiz>> UpdateQuiz([Required] int quizId, QuizCreationDto quiz)
    {
        return Ok(await quizService.UpdateAsync(quizId, quiz));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{quizId}")]
    public async Task<ActionResult<bool>> DeleteQuiz(uint quizId)
    {
        return Ok(await quizService.DeleteAsync(quiz => quiz.Id == quizId));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{quizId}/questions")]
    public async Task<ActionResult<bool>> RemoveQuizQuestions(int quizId, int[] questionIds)
    {
        if (questionIds.Length < 1) { return BadRequest("Provide at least one Question Id"); }

        return Ok(await quizService.RemoveQuizQuestionsAsync(quizId, questionIds));
    }
}