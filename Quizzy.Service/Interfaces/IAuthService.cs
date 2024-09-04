using Quizzy.Service.DTOs.User;

namespace Quizzy.Service.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(UserLoginDto loginDto);
    Task<UserViewDto> RegisterAsync(UserCreationDto userCreationDto);
}
