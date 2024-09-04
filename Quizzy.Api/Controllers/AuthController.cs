using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quizzy.Domain.Entities;
using Quizzy.Service.DTOs.User;
using Quizzy.Service.Interfaces;

namespace Quizzy.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("sign-up")]
    public async Task<ActionResult<User>> SingUp(UserCreationDto userCreation)
    {
        return Ok(await authService.RegisterAsync(userCreation));
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserLoginDto loginDto)
    {
        return Ok(await authService.LoginAsync(loginDto));
    }
}
