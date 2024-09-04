using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizzy.Domain.Commons;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.User;
using Quizzy.Service.Helpers;
using Quizzy.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Quizzy.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUserService userService) : ControllerBase
{
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers(
        [FromQuery] PaginationParameters? parameters)
    {
        return Ok(await userService.GetAllAsync(parameters: parameters));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("{userId}")]
    public async Task<ActionResult<User>> GetUser(int userId)
    {
        return Ok(await userService.GetAsync(userId));
    }

    [Authorize(Policy = "UserPolicy")]
    [HttpGet("me")]
    public async Task<ActionResult<User>> GetAboutMe()
    {
        return Ok(await userService.GetAsync(HttpContextHelper.UserId));
    }

    [Authorize(Policy = "UserPolicy")]
    [HttpPatch("{userId}")]
    public async Task<ActionResult<UserViewDto>> UpdateUser([Required] int userId, User user)
    {
        return Ok(await userService.UpdateAsync(userId, user));
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{userId}")]
    public async Task<ActionResult<bool>> DeleteUser(uint userId)
    {
        return Ok(await userService.DeleteAsync(user => user.Id == userId));
    }
}